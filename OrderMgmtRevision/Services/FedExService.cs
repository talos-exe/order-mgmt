using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OrderMgmtRevision.Models;

public class FedExService
{
    private readonly string _apiKey;
    private readonly string _apiSecret;
    private readonly string _accountNumber;
    private readonly string _baseUrl;

    public FedExService(IConfiguration configuration)
    {
        var fedExSection = configuration.GetSection("FedEx");
        _apiKey = fedExSection["ApiKey"];
        _apiSecret = fedExSection["ApiSecret"];
        _accountNumber = fedExSection["AccountNumber"];
        _baseUrl = fedExSection["BaseUrl"];
    }

    public async Task<string> GetAccessTokenAsync()
    {
        using (var client = new HttpClient())
        {
            var tokenUrl = $"{_baseUrl}/oauth/token";
            var requestBody = $"grant_type=client_credentials&client_id={_apiKey}&client_secret={_apiSecret}";
            var content = new StringContent(requestBody, Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await client.PostAsync(tokenUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic tokenData = JsonConvert.DeserializeObject(jsonResponse);
                return tokenData.access_token;
            }
            else
            {
                throw new Exception($"Failed to get token: {response.ReasonPhrase}");
            }
        }
    }

    public async Task<string> CreateShipmentAsync(string accessToken, FedExShipment shipment)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var createShipmentUrl = $"{_baseUrl}/ship/v1/shipments";

            var createShipmentRequest = new // hardcoded for testing
            {
                requestedShipment = new
                {
                    shipper = new
                    {
                        address = new
                        { 
                            streetLines = new[] { "123 Jennings St" }, 
                            city = "Newburgh", 
                            stateOrProvinceCode = "IN", 
                            postalCode = "47630", 
                            countryCode = "US" 
                        },
                        contact = new 
                        { 
                            personName = "Rachel Basinski", 
                            phoneNumber = "1234567890" 
                        }
                    },
                    recipients = new[]
                    {
                        new {
                            address = new 
                            { 
                                streetLines = new[] { shipment.Address }, 
                                city = shipment.City, 
                                stateOrProvinceCode = shipment.State, 
                                postalCode = shipment.PostalCode, 
                                countryCode = shipment.CountryCode 
                            },
                            contact = new 
                            { 
                                personName = shipment.RecipientName, 
                                phoneNumber = shipment.PhoneNumber 
                            }
                        }
                    },
                    pickupType = "DROPOFF_AT_FEDEX_LOCATION",
                    serviceType = "STANDARD_OVERNIGHT",
                    packagingType = "FEDEX_PAK",
                    totalWeight = 10.6,
                    shippingChargesPayment = new 
                    { 
                        paymentType = "SENDER" 
                    },
                    labelSpecification = new 
                    {
                        labelStockType = "PAPER_4X6",
                        imageType = "PDF"
                    },
                    requestedPackageLineItems = new[]
                    {
                        new 
                        { 
                            weight = new 
                            { 
                                units = "LB", 
                                value = "10" 
                            }
                        }
                    }
                },
                labelResponseOptions = "LABEL",
                accountNumber = new 
                { 
                    value = _accountNumber 
                }
            };

            var jsonRequest = JsonConvert.SerializeObject(createShipmentRequest);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(createShipmentUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Failed to create shipment: {response.ReasonPhrase}");            
            }
        }
    }

    public async Task<string> CancelShipmentAsync(string accessToken, string trackingNumber)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var cancelShipmentUrl = $"{_baseUrl}/ship/v1/shipments/cancel";

            var cancelShipmentRequest = new
            {
                accountNumber = new 
                {
                  value = _accountNumber
                },
                trackingNumber = trackingNumber
            };

            var jsonRequest = JsonConvert.SerializeObject(cancelShipmentRequest);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(cancelShipmentUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Failed to cancel shipment: {response.ReasonPhrase}");            
            }
        }
    }

}