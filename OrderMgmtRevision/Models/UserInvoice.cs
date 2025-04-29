using System.ComponentModel.DataAnnotations;

namespace OrderMgmtRevision.Models
{
    public class UserInvoice
    {
        public Guid Id { get; set; } // Primary key for invoice
            
        public string? Description { get; set; }
        public decimal InvoiceAmount { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public bool IsPaid { get; set; } = false;

        public DateTime DateDue { get; set; } = DateTime.UtcNow.AddDays(7);

        public DateTime? DatePaid { get; set; }

        public string? PaymentReference { get; set; }

        // Foreign key to User
        public string? UserId { get; set; }   // User primary key (string because Identity uses string PKs)
        public User? User { get; set; }

        // Foreign key to Shipment
        public int? ShipmentId { get; set; }
        public Shipment? Shipment { get; set; }
    }
}
