
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementSystem.Models
{
    public class Order
    {
        [Key]
        [Column("Order_ID")]
        [StringLength(25)]
        public string OrderId { get; set; }

        [Required]
        [Column("User_ID")]
        [StringLength(25)]
        public string UserId { get; set; }

        [Column("TotalAmount", TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        [Column("OrderDate")]
        public DateTime OrderDate { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public ICollection<OrderItem> OrderItem { get; set; } = new List<OrderItem>();
    }
}
