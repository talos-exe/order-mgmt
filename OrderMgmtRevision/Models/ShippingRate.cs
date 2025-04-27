using Microsoft.EntityFrameworkCore;

namespace OrderMgmtRevision.Models
{
    [Owned]
    public class ShippingRate
    {
        public string RateObjectId { get; set; }
        public string Provider { get; set; }
        public string Service { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public int? EstimatedDays { get; set; }
    }
}
