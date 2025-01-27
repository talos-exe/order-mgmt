
using OrderManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementSystem.Models
{
    public class ParcelOutbound
    {
        [Key]
        [Column("Order_ID")]
        [StringLength(25)]
        public string OrderId { get; set; }

        [Column("Order_Type")]
        [StringLength(25)]
        public string OrderType { get; set; }

        [Column("Order_Status")]
        [StringLength(25)]
        public string OrderStatus { get; set; }

        [Required]
        [Column("Warehouse_ID")]
        [StringLength(25)]
        public string Warehouse_ID { get; set; }

        [Required]
        [Column("User_ID")]
        [StringLength(25)]
        public string UserId { get; set; }

        [Column("Platform")]
        [StringLength(50)]
        public string Platform { get; set; }

        [Column("Estimated_Delivery_Date")]
        public DateTime EstimatedDeliveryDate { get; set; }

        [Column("Ship_Date")]
        public DateTime ShipDate { get; set; }

        [Column("Transport_Days")]
        public int TransportDays { get; set; }

        [Column("Cost", TypeName = "decimal(10, 2)")]
        public decimal Cost { get; set; }

        [Column("Currency")]
        [StringLength(25)]
        public string Currency { get; set; }

        [Column("Recipient")]
        [StringLength(50)]
        public string Recipient { get; set; }

        [Column("Country")]
        [StringLength(50)]
        public string Country { get; set; }

        [Column("Postcode")]
        [StringLength(25)]
        public string Postcode { get; set; }

        [Column("Tracking_Number")]
        [StringLength(25)]
        public string TrackingNumber { get; set; }

        [Column("Reference_Order_Number")]
        [StringLength(25)]
        public string ReferenceOrderNumber { get; set; }

        [Column("Creation_Date")]
        public DateTime CreationDate { get; set; }

        [Column("Boxes")]
        public int Boxes { get; set; }

        [Column("Shipping_Company")]
        [StringLength(50)]
        public string ShippingCompany { get; set; }

        [Column("Latest_Information")]
        [StringLength(255)]
        public string LatestInformation { get; set; }

        [Column("Tracking_Update_Time")]
        public DateTime TrackingUpdateTime { get; set; }

        [Column("Internet_Posting_Time")]
        public DateTime InternetPostingTime { get; set; }

        [Column("Delivery_Time")]
        public DateTime DeliveryTime { get; set; }

        [Column("Related_Adjustment_Order")]
        [StringLength(25)]
        public string RelatedAdjustmentOrder { get; set; }

        // Navigation Properties
        public User User { get; set; }

        public Warehouse Warehouse { get; set; }

        public ICollection<ParcelProductList> ParcelProductList { get; set; } = new List<ParcelProductList>();
    }
}

