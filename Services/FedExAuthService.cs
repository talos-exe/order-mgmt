using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

public class FedExAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public FedExAuthService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> GetOAuthTokenAsync()
    {
        // Fetch the OAuth URL from appsettings.json
        var apiUrl = _configuration["FedExApi:BaseUrl"] + _configuration["FedExApi:OAuthUrl"];

        // Fetch Client ID and Client Secret from appsettings.json
        var clientId = _configuration["FedExApi:ClientId"];
        var clientSecret = _configuration["FedExApi:ClientSecret"];

        // Prepare the request data
        var requestData = new
        {
            grant_type = "client_credentials",
            client_id = clientId,
            client_secret = clientSecret
        };

        var jsonData = JsonConvert.SerializeObject(requestData);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        // Make the OAuth request
        var response = await _httpClient.PostAsync(apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            var tokenData = JsonConvert.DeserializeObject<dynamic>(result);
            return tokenData.access_token; // Return the OAuth token
        }

        return null; // Handle failure
    }
}