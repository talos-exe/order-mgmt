namespace OrderMgmtRevision.Models
{
    public class StripePaymentModel
    {
        public long Amount { get; set; } // Amount in cents
        public string StripePublishableKey { get; set; }
    }

    public class SavePaymentMethodRequest
    {
        public string PaymentMethodId { get; set; }
        public string Email { get; set; }
    }

    public class CreateCheckoutRequest
    {
        public string CustomerId { get; set; }
        public long Amount { get; set; }
        public string PaymentType { get; set; } // "card" or "ach"
    }
}