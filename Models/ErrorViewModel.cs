namespace OrderManagementSystem.Models
{
    public class ErrorViewModel
    {
        // RequestId property can be null or empty
        public string? RequestId { get; set; }

        // Property to determine if the RequestId should be shown
        public bool ShowRequestId => !string.IsNullOrWhiteSpace(RequestId);
    }
}