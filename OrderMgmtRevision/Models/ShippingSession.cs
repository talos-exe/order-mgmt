namespace OrderMgmtRevision.Models
{
    public class ShippingSession
    {
        public ShippingRequest Request { get; set; }
        public List<ShippingRate> Rates { get; set; }
        public string SelectedRateId {  get; set; }
        public string TrackingNumber { get; set; }
        public string LabelUrl {  get; set; }

    }
}
