namespace OrderManagementSystem.Models
{
    public class FedExSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string BaseUrl { get; set; }
        public string OAuthUrl { get; set; }
        public string ShipmentUrl { get; set; }
    }
}
