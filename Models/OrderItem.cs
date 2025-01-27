using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementSystem.Models
{
    public class OrderItem
    {
        [Required]
        [StringLength(25)]
        public string OrderId { get; set; }

        [Required]
        [StringLength(25)]
        public string ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double UnitPrice { get; set; }

        // Navigation Properties
        public Order Order { get; set; }
        public Inventory Inventory { get; set; }
    }
}