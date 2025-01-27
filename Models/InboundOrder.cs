
using OrderManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementSystem.Models
{
    public class InboundOrder
    {
        [Key]
        [Column("Inbound_Order_ID")]
        [StringLength(25)]
        public string InboundOrderId { get; set; }

        [Column("Order_Status")]
        [StringLength(25)]
        public string OrderStatus { get; set; }

        [Required]
        [Column("User_ID")]
        [StringLength(25)]
        public string User_ID { get; set; }

        [Required]
        [Column("Warehouse_ID")]
        [StringLength(25)]
        public string Warehouse_ID { get; set; }

        [Column("Estimated_Arrival")]
        public DateTime EstimatedArrival { get; set; }

        [Column("Product_Quantity")]
        public int ProductQuantity { get; set; }

        [Column("Creation_Date")]
        public DateTime CreationDate { get; set; }

        [Column("Cost", TypeName = "decimal(10, 2)")]
        public decimal Cost { get; set; }

        [Column("Currency")]
        [StringLength(50)]
        public string Currency { get; set; }

        [Column("Boxes")]
        public int Boxes { get; set; }

        [Column("Inbound_Type")]
        [StringLength(25)]
        public string InboundType { get; set; }

        [Column("Tracking_Number")]
        [StringLength(255)]
        public string TrackingNumber { get; set; }

        [Column("Reference_Order_Number")]
        [StringLength(255)]
        public string ReferenceOrderNumber { get; set; }

        [Column("Arrival_Method")]
        [StringLength(25)]
        public string ArrivalMethod { get; set; }

        // Navigation Properties
        public User User { get; set; }

       
        public Warehouse Warehouse { get; set; }

        public ICollection<InboundProductList> InboundProductList { get; set; } = new List<InboundProductList>();
    }
}

