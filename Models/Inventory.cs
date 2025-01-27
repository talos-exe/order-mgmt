using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementSystem.Models
{
    public class Inventory
    {
        [Key]
        [Column("Product_ID")]
        [StringLength(25)]
        public string Product_ID { get; set; }

        [Required]
        [Column("Warehouse_ID")]
        [StringLength(25)]
        public string Warehouse_ID { get; set; }

        [Column("SKU")]
        [StringLength(50)]
        public string SKU { get; set; }

        [Column("Product_Name")]
        [StringLength(255)]
        public string Product_Name { get; set; }

        [Column("Product_Description")]
        [StringLength(255)]
        public string Product_Description { get; set; }

        [Column("Price", TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be positive.")]
        public decimal Price { get; set; }

        [Column("Quantity")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int Quantity { get; set; }

        // Navigation Properties
        public Warehouse Warehouse { get; set; } 
        public ICollection<InboundProductList> InboundProductList { get; set; } = new List<InboundProductList>();
        public ICollection<FreightProductList> FreightProductList { get; set; } = new List<FreightProductList>();
        public ICollection<ParcelProductList> ParcelProductList { get; set; } = new List<ParcelProductList>();
        public ICollection<OrderItem> OrderItem { get; set; } = new List<OrderItem>();
        public ICollection<PlatformProductList> PlatformProductList { get; set; } = new List<PlatformProductList>();
    }
}