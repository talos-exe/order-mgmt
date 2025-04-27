using Newtonsoft.Json;
using OrderMgmtRevision.Models;
using System.Net.Http.Headers;
using static System.Net.WebRequestMethods;

namespace OrderMgmtRevision.Services
{
    public class ShippoService
    {

        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiUrl;

        public ShippoService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            var shippoSec = configuration.GetSection("Shippo");
            _apiKey = shippoSec["ApiKey"];
            _apiUrl = "https://api.goshippo.com/";

            _httpClient.BaseAddress = new Uri(_apiUrl);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("ShippoToken", _apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public class AddressValidationResult
        {
            public bool IsValid { get; set; }
            public List<AddressSuggestion> Suggestions { get; set; } = new List<AddressSuggestion>();
        }

        public class AddressSuggestion
        {
            public string Street1 { get; set; }
            public string Street2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
            public string Country { get; set; }
        }


        public async Task<List<ShippingRate>> GetShippingRatesAsync(ShippingRequest request)
        {
            var shippoRequest = new
            {
                address_from = new
                {
                    name = request.FromName,
                    street1 = request.FromStreet,
                    city = request.FromCity,
                    state = request.FromState,
                    zip = request.FromZip,
                    country = "US",
                    phone = request.FromPhone,
                    email = request.FromEmail
                },
                address_to = new
                {
                    name = request.ToName,
                    street1 = request.ToStreet,
                    city = request.ToCity,
                    state = request.ToState,
                    zip = request.ToZip,
                    country = request.ToCountryCode
                },
                parcels = new[]
                {
                new
                {
                    length = request.Length.ToString(),
                    width = request.Width.ToString(),
                    height = request.Height.ToString(),
                    distance_unit = "in",
                    weight = request.Weight.ToString(),
                    mass_unit = "lb"
                }
            },
                async = false
            };

            var jsonContent = JsonConvert.SerializeObject(shippoRequest);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("shipments/", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine("Shippo Request response: " + responseContent);
            dynamic shippoResponse = JsonConvert.DeserializeObject(responseContent);

            var rates = new List<ShippingRate>();
            foreach (var rate in shippoResponse.rates)
            {
                rates.Add(new ShippingRate
                {
                    Provider = rate.provider,
                    Service = rate.servicelevel.name,
                    Amount = decimal.Parse(rate.amount.ToString()),
                    Currency = rate.currency,
                    EstimatedDays = rate.estimated_days,
                    RateObjectId = rate.object_id
                });
            }

            return rates;
        }

        public async Task<ShippingLabel> CreateLabelAsync(string rateObjectId)
        {
            var transactionRequest = new
            {
                rate = rateObjectId,
                async = false
            };

            var jsonContent = JsonConvert.SerializeObject(transactionRequest);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync("transactions/", content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Request failed: {ex.Message}");
                throw; // Re-throw the exception after logging
            }

            //var response = await _httpClient.PostAsync("transactions/", content);
            //response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic transactionResponse = JsonConvert.DeserializeObject(responseContent);
            System.Diagnostics.Debug.WriteLine("Shippo Label response:" + responseContent);

            return new ShippingLabel
            {
                LabelUrl = transactionResponse.label_url,
                LabelObjectId = transactionResponse.object_id,
                TrackingNumber = transactionResponse.tracking_number,
                TrackingUrl = transactionResponse.tracking_url_provider
                //Carrier = transactionResponse.tracking_provider,
                //Amount = decimal.Parse(transactionResponse.amount.ToString()),
                //Currency = transactionResponse.currency
                // Uncomment above in actual test key.
            };
        }

        public async Task<ShipmentTracking> TrackShipmentAsync(string carrier, string trackingNumber)
        {
            var response = await _httpClient.GetAsync($"tracks/{carrier}/{trackingNumber}");
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic trackingResponse = JsonConvert.DeserializeObject(responseContent);

            return new ShipmentTracking
            {
                Status = trackingResponse.tracking_status?.status,
                StatusDate = trackingResponse.tracking_status?.status_date,
                Location = $"{trackingResponse.tracking_status?.location?.city}, {trackingResponse.tracking_status?.location?.state}"
            };
        }

        public async Task<bool> CancelShipmentAsync(string objectid)
        {
            try
            {
                // Create a properly formatted request body
                var requestBody = new Dictionary<string, string>
                    {
                        { "transaction", objectid }
                    };

                var content = new StringContent(
                    JsonConvert.SerializeObject(requestBody),
                    System.Text.Encoding.UTF8,
                    "application/json");

                // Try the refunds endpoint instead of the transaction-specific endpoint
                var response = await _httpClient.PostAsync("refunds", content);

                var responseContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"Cancel response: {responseContent}");

                if (!response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"Cancel failed with status code: {response.StatusCode}");
                    System.Diagnostics.Debug.WriteLine($"Response content: {responseContent}");
                    return false;
                }

                dynamic result = JsonConvert.DeserializeObject(responseContent);
                string status = result?.status?.ToString();

                System.Diagnostics.Debug.WriteLine($"Refund status: {status}");
                return !string.IsNullOrEmpty(status) &&
                       (status.Equals("REFUNDED", StringComparison.OrdinalIgnoreCase) ||
                        status.Equals("QUEUED", StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception during cancellation: {ex.Message}");
                return false;
            }
        }

        public async Task<AddressValidationResult> ValidateAddressAsync(Dictionary<string, object> addressData)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"ShippoToken {_apiKey}");

            addressData["validate"] = true;
            addressData["validation_results"] = new Dictionary<string, object>
            {
                ["allow_distance_threshold"] = true,  // Allow addresses within a reasonable distance
                ["distance_threshold"] = 10,          // Distance in miles
                ["return_alternatives"] = true,       // Ensure we get alternative addresses
                ["return_fuzzy_alternatives"] = true  // Include less exact matches
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(addressData),
                System.Text.Encoding.UTF8,
                "application/json");

            System.Diagnostics.Debug.WriteLine($"Sending address validation request: {JsonConvert.SerializeObject(addressData)}");

            var response = await client.PostAsync("https://api.goshippo.com/addresses/", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            System.Diagnostics.Debug.WriteLine($"Shippo address validation response: {responseContent}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Shippo API Error: {responseContent}");
            }

            dynamic addressResponse = JsonConvert.DeserializeObject(responseContent);

            var result = new AddressValidationResult
            {
                IsValid = addressResponse.validation_results?.is_valid ?? false
            };

            // Add suggestions if available
            if (addressResponse.validation_results?.address_alternatives != null)
            {
                foreach (var alt in addressResponse.validation_results.address_alternatives)
                {
                    result.Suggestions.Add(new AddressSuggestion
                    {
                        Street1 = alt.street1 ?? "",
                        Street2 = alt.street2 ?? "",
                        City = alt.city ?? "",
                        State = alt.state ?? "",
                        Zip = alt.zip ?? "",
                        Country = alt.country ?? "US"
                    });
                }
            }

            // If the address is valid but no alternatives were provided,
            // add the validated address as a suggestion
            if (result.Suggestions.Count == 0)
            {
                // Add the original validated address as a suggestion
                result.Suggestions.Add(new AddressSuggestion
                {
                    Street1 = addressResponse.street1 ?? "",
                    Street2 = addressResponse.street2 ?? "",
                    City = addressResponse.city ?? "",
                    State = addressResponse.state ?? "",
                    Zip = addressResponse.zip ?? "",
                    Country = addressResponse.country ?? "US"
                });
            }

            return result;
        }

    }


}
