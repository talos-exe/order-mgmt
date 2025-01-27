using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrderManagementSystem.Models;

public class FedExShippingService
{
    private readonly HttpClient _httpClient;
    private readonly FedExAuthService _authService;
    private readonly FedExSettings _fedExSettings; // Store FedEx API settings

    // Constructor injection of FedExSettings and other services
    public FedExShippingService(HttpClient httpClient, FedExAuthService authService, IOptions<FedExSettings> fedExSettings)
    {
        _httpClient = httpClient;
        _authService = authService;
        _fedExSettings = fedExSettings.Value; // Access the settings values
    }

    // Method to create a shipment using FedEx API
    public async Task<string> CreateShipmentAsync(FedExShipmentData shipmentData)
    {
        // Get OAuth token
        var token = await _authService.GetOAuthTokenAsync();
        if (token == null)
        {
            return "Failed to authenticate with FedEx.";
        }

        // Fetch the shipment creation URL from FedExSettings
        var apiUrl = _fedExSettings.BaseUrl + _fedExSettings.ShipmentUrl;

        // Add the OAuth token to the request headers
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        // Serialize shipment data to JSON
        var jsonData = JsonConvert.SerializeObject(shipmentData);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        // Send the shipment creation request
        var response = await _httpClient.PostAsync(apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync(); // Return shipment details
        }

        return "Shipment creation failed"; // Handle errors
    }
}