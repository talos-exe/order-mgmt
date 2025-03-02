namespace OrderMgmtRevision.Models
{
    public class FedExShipment
    {
        public string? TrackingNumber { get; set; }
        public string RecipientName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}