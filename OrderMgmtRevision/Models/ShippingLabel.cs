using Microsoft.EntityFrameworkCore;

namespace OrderMgmtRevision.Models
{
    [Owned]
    public class ShippingLabel
    {
        public string LabelUrl { get; set; }
        public string LabelObjectId { get; set; }
        public string TrackingNumber { get; set; }
        public string TrackingUrl { get; set; }
    }
}
