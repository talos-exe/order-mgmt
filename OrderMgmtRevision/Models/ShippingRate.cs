namespace OrderMgmtRevision.Models
{
    public class ShippingRate
    {
        public string Provider { get; set; }
        public string Service { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public int EstimatedDays { get; set; }
    }
}
