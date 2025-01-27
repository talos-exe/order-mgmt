
using OrderManagementSystem.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementSystem.Models
{
    public class Customer
    {
        [Key]
        [Column("User_ID")]
        [StringLength(25)]
        public string UserId { get; set; }

        [Column("Admin_ID")]
        [StringLength(25)]
        public string AdminId { get; set; }

        [Column("Company_Name")]
        [StringLength(50)]
        public string CompanyName { get; set; }

        [Column("Account_Status")]
        [StringLength(25)]
        public string AccountStatus { get; set; }

        [Column("Product_Need_Audit_Free")]
        [StringLength(25)]
        public string ProductNeedAuditFree { get; set; }

        [Column("Warehouse_Availability")]
        public int WarehouseAvailability { get; set; }

        [Column("Billing_Account_ID")]
        [StringLength(25)]
        public string BillingAccountId { get; set; }

        [Column("Date_Created")]
        public DateTime DateCreated { get; set; }

        // Navigation Properties
        
        public User Administrator { get; set; }

        public User CustomerUser { get; set; }

        public BillingAccount BillingAccount { get; set; }
    }
}
