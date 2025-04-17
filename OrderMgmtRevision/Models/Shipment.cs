using System.ComponentModel.DataAnnotations;

namespace OrderMgmtRevision.Models
{
    public class Shipment
    {
        public int? ShipmentID { get; set; }
        
        [Required]
        [MaxLength(36)]
        public string? ShipmentName { get; set; } = "New Shipment";

        public string ProductID { get; set; }

        public int? Quantity { get; set; }

        public int SourceWarehouseID { get; set; }

        //public int? DestinationWarehouseID { get; set; }

        public string? Status { get; set; } = "In Progress";

        public DateTime ShipmentDate { get; set; } = DateTime.Now;

        public DateTime? EstimatedArrival { get; set; }

        public decimal Cost { get; set; } = 0; // Shipping cost, 0 if null

        public string? TrackingNumber { get; set; }  // Shipping Tracking Number

        public DateTime GeneratedAt { get; set; } = DateTime.Now;

        // Relationships
        public Product Product { get; set; }

        public Warehouse SourceWarehouse { get; set; }

        //public Warehouse DestinationWarehouse { get; set; }

        public string SelectedRateId { get; set; }

        public ShippingRate Rate { get; set; }

        public ShippingLabel Label { get; set; }

        public ShipmentTracking Tracking { get; set; }

        public ShippingRequest ShippingRequest { get; set; }

        public ICollection<ShipmentStatusHistory> StatusHistory {  get; set; }


    }
}
