using OrderManagementSystem.Models;

public class FedExShipmentData
{
    public Shipper Shipper { get; set; }
    public List<Recipient> Recipients { get; set; }
    public string ShipDatestamp { get; set; }
    public string ServiceType { get; set; }
    public string PackagingType { get; set; }
    public string PickupType { get; set; }
    public ShippingChargesPayment ShippingChargesPayment { get; set; }
    public ShipmentSpecialServices ShipmentSpecialServices { get; set; }
    public LabelSpecification LabelSpecification { get; set; }
    public List<RequestedPackageLineItem> RequestedPackageLineItems { get; set; }
}