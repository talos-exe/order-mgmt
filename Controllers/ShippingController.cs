using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSystem.Models; // Ensure this is correct namespace

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly FedExSettings _fedExSettings;

        public ShippingController(IHttpClientFactory httpClientFactory, IOptions<FedExSettings> fedExSettings)
        {
            _httpClient = httpClientFactory.CreateClient();
            _fedExSettings = fedExSettings.Value;
        }

        [HttpPost("CreateShippingLabel")]
        public async Task<IActionResult> CreateShippingLabel([FromBody] FedExLabelRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return BadRequest(new { errors });
            }

            var token = await GetFedExAccessTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return StatusCode(500, "Failed to retrieve FedEx access token.");
            }

            var fedExRequest = MapToFedExShipmentRequest(request);
            var requestContent = new StringContent(JsonConvert.SerializeObject(fedExRequest), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync(_fedExSettings.ShipmentUrl, requestContent);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseData);

                if (jsonResponse.output?.transactionShipments?[0]?.pieceResponses?[0]?.packageDocuments?[0]?.url != null)
                {
                    string labelUrl = jsonResponse.output.transactionShipments[0].pieceResponses[0].packageDocuments[0].url;
                    return Ok(new { labelUrl });
                }
                else
                {
                    return StatusCode(500, "Label URL not found in FedEx response.");
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorContent);
            }
        }

        private async Task<string> GetFedExAccessTokenAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _fedExSettings.OAuthUrl)
                {
                    Content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("grant_type", "client_credentials"),
                        new KeyValuePair<string, string>("client_id", _fedExSettings.ClientId),
                        new KeyValuePair<string, string>("client_secret", _fedExSettings.ClientSecret)
                    })
                };

                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                dynamic json = JsonConvert.DeserializeObject(responseContent);
                return json.access_token;
            }
            catch
            {
                return null;
            }
        }

        private static object MapToFedExShipmentRequest(FedExLabelRequest request)
        {
            return new
            {
                labelResponseOptions = "URL_ONLY",
                accountNumber = new { value = request.AccountNumber },
                requestedShipment = new
                {
                    shipper = new
                    {
                        contact = new
                        {
                            personName = request.OriginAddress.Contact.PersonName,
                            companyName = request.OriginAddress.Contact.CompanyName,
                            phoneNumber = request.OriginAddress.Contact.PhoneNumber
                        },
                        address = new
                        {
                            streetLines = request.OriginAddress.StreetLines,
                            city = request.OriginAddress.City,
                            stateOrProvinceCode = request.OriginAddress.StateOrProvinceCode,
                            postalCode = request.OriginAddress.PostalCode,
                            countryCode = request.OriginAddress.CountryCode
                        }
                    },
                    recipients = new[]
                    {
                        new
                        {
                            contact = new
                            {
                                personName = request.DestinationAddress.Contact.PersonName,
                                companyName = request.DestinationAddress.Contact.CompanyName,
                                phoneNumber = request.DestinationAddress.Contact.PhoneNumber
                            },
                            address = new
                            {
                                streetLines = request.DestinationAddress.StreetLines,
                                city = request.DestinationAddress.City,
                                stateOrProvinceCode = request.DestinationAddress.StateOrProvinceCode,
                                postalCode = request.DestinationAddress.PostalCode,
                                countryCode = request.DestinationAddress.CountryCode
                            }
                        }
                    },
                    shipDatestamp = request.ShipDateStamp,
                    serviceType = request.ServiceType,
                    packagingType = request.PackagingType,
                    pickupType = request.PickupType,
                    blockInsightVisibility = false,
                    shippingChargesPayment = new { paymentType = "SENDER" },
                    shipmentSpecialServices = new { specialServiceTypes = new string[] { } },
                    labelSpecification = new
                    {
                        labelFormatType = "COMMON2D",
                        imageType = "PDF",
                        labelStockType = "PAPER_85X11_TOP_HALF_LABEL"
                    },
                    requestedPackageLineItems = new[]
                    {
                        new
                        {
                            weight = new { units = "LB", value = request.PackageDetails.Weight },
                            dimensions = new {
                                length = request.PackageDetails.Dimensions.Length,
                                width = request.PackageDetails.Dimensions.Width,
                                height = request.PackageDetails.Dimensions.Height,
                                units = "IN"
                            },
                            sequenceNumber = 1
                        }
                    }
                }
            };
        }
    }
}
  