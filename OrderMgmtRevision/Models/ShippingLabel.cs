using Microsoft.EntityFrameworkCore;

namespace OrderMgmtRevision.Models
{
    [Owned]
    public class ShippingLabel
    {
        public string LabelUrl { get; set; }
        public string TrackingNumber { get; set; }
        public string TrackingUrl { get; set; }
        public string Carrier { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
