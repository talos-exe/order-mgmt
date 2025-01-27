
using OrderManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementSystem.Models
{
    public class FreightOutbound
    {
        [Key]
        [Column("Outbound_Order_ID")]
        [StringLength(25)]
        public string OutboundOrderId { get; set; }

        [Column("Order_Type")]
        [StringLength(25)]
        public string OrderType { get; set; }

        [Column("Order_Status")]
        [StringLength(25)]
        public string OrderStatus { get; set; }

        [Required]
        [Column("User_ID")]
        [StringLength(25)]
        public string UserId { get; set; }

        [Required]
        [Column("Warehouse_ID")]
        [StringLength(25)]
        public string Warehouse_ID { get; set; }

        [Column("Product_Quantity")]
        public int ProductQuantity { get; set; }

        [Column("Creation_Date")]
        public DateTime CreationDate { get; set; }

        [Column("Estimated_Delivery_Date")]
        public DateTime EstimatedDeliveryDate { get; set; }

        [Column("Order_Ship_Date")]
        public DateTime OrderShipDate { get; set; }

        [Column("Cost", TypeName = "decimal(10, 2)")]
        public decimal Cost { get; set; }

        [Column("Currency")]
        [StringLength(50)]
        public string Currency { get; set; }

        [Column("Recipient")]
        [StringLength(100)]
        public string Recipient { get; set; }

        [Column("Recipient_Post_Code")]
        [StringLength(50)]
        public string RecipientPostCode { get; set; }

        [Column("Destination_Type")]
        [StringLength(50)]
        public string DestinationType { get; set; }

        [Column("Platform")]
        [StringLength(50)]
        public string Platform { get; set; }

        [Column("Shipping_Company")]
        [StringLength(50)]
        public string ShippingCompany { get; set; }

        [Column("Transport_Days")]
        public int TransportDays { get; set; }

        [Column("Related_Adjustment_Order")]
        [StringLength(25)]
        public string RelatedAdjustmentOrder { get; set; }

        [Column("Tracking_Number")]
        [StringLength(255)]
        public string TrackingNumber { get; set; }

        [Column("Reference_Order_Number")]
        [StringLength(255)]
        public string ReferenceOrderNumber { get; set; }

        [Column("FBA_Shipment_ID")]
        [StringLength(25)]
        public string FBAShipmentId { get; set; }

        [Column("FBA_Tracking_Number")]
        [StringLength(25)]
        public string FBATrackingNumber { get; set; }

        [Column("Outbound_Method")]
        [StringLength(25)]
        public string OutboundMethod { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public Warehouse Warehouse { get; set; }

        public ICollection<FreightProductList> FreightProductList { get; set; } = new List<FreightProductList>();
    }
}

