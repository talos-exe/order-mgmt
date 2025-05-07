namespace OrderMgmtRevision.Models
{
    public class ShipmentViewModel
    {
        public int? ShipmentID { get; set; }
        public string RecipientName { get; set; } = "No Recipient";
        public string? TrackingNumber { get; set; } = "No Tracking Number";

        public string Address { get; set; } = "No Address";
        public string City { get; set; } = "N/A";
        public string CountryCode { get; set; } = "US";
        public string State { get; set; } = "N/A";
        public string PhoneNumber { get; set; } = "N/A";
        public string PostalCode { get; set; } = "N/A";

        public string? Status { get; set; } = "UNKNOWN";

        public Shipment Shipment { get; set; } = new Shipment();
        public List<ShipmentStatusHistory> ShipmentLogs { get; set; }
    }
}
