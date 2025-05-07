using System.ComponentModel.DataAnnotations;

namespace OrderMgmtRevision.Models
{
    public class ShipmentStatusHistory
    {
        public int ShipmentStatusHistoryId { get; set; }

        public int? ShipmentID { get; set; }  // Foreign Key
        public Shipment? Shipment { get; set; }

        public string? Status { get; set; }

        public string? Note { get; set; }

        public string? Location { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
