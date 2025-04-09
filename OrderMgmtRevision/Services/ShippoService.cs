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
                    country = "US"
                },
                address_to = new
                {
                    name = request.ToName,
                    street1 = request.ToStreet,
                    city = request.ToCity,
                    state = request.ToState,
                    zip = request.ToZip,
                    country = "US"
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
                    EstimatedDays = rate.estimated_days
                });
            }

            return rates;
        }

    }
}
