
using OrderManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementSystem.Models
{
    public class PlatformOrder
    {
        [Key]
        [Column("Order_ID")]
        [StringLength(25)]
        public string OrderId { get; set; }

        [Column("Platform")]
        [StringLength(25)]
        public string Platform { get; set; }

        [Required]
        [Column("Warehouse_ID")]
        [StringLength(25)]
        public string Warehouse_ID { get; set; }

        [Column("Product_Quantity")]
        public int ProductQuantity { get; set; }

        [Required]
        [Column("User_ID")]
        [StringLength(25)]
        public string UserId { get; set; }

        [Column("Buyer")]
        [StringLength(25)]
        public string Buyer { get; set; }

        [Column("Recipient_Postcode")]
        [StringLength(25)]
        public string RecipientPostcode { get; set; }

        [Column("Recipient_Country")]
        [StringLength(25)]
        public string RecipientCountry { get; set; }

        [Column("Store")]
        [StringLength(25)]
        public string Store { get; set; }

        [Column("Site")]
        [StringLength(25)]
        public string Site { get; set; }

        [Column("Shipping_Service")]
        [StringLength(25)]
        public string ShippingService { get; set; }

        [Column("Tracking_Number")]
        [StringLength(255)]
        public string TrackingNumber { get; set; }

        [Column("Carrier")]
        [StringLength(25)]
        public string Carrier { get; set; }

        [Column("Order_Time")]
        public DateTime OrderTime { get; set; }

        [Column("Payment_Time")]
        public DateTime PaymentTime { get; set; }

        [Column("Created_Time")]
        public DateTime CreatedTime { get; set; }

        [Column("Order_Source")]
        [StringLength(25)]
        public string OrderSource { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public Warehouse Warehouse { get; set; }
        public ICollection<PlatformProductList> PlatformProductList { get; set; } = new List<PlatformProductList>();
    }
}
