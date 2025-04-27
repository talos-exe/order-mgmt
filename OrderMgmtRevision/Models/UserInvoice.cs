namespace OrderMgmtRevision.Models
{
    public class UserInvoice
    {
        public Guid Id { get; set; } // Primary key for invoice
            
        public string? Description { get; set; }
        public decimal InvoiceAmount { get; set; }

        // Foreign key to User
        public string? UserId { get; set; }   // User primary key (string because Identity uses string PKs)
        public User? User { get; set; }

        // Foreign key to Shipment
        public int? ShipmentId { get; set; }
        public Shipment? Shipment { get; set; }
    }
}
