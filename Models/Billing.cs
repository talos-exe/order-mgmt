
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementSystem.Models
{
    public class Billing
    {
        [StringLength(25)]
        public string BillingAccountId { get; set; }

        [StringLength(25)]
        public string ChargeId { get; set; }

        [Column("Amount", TypeName = "decimal(10, 2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be positive.")]
        public decimal Amount { get; set; }

        [Column("Date_Created")]
        public DateTime DateCreated { get; set; }

       //Navigation Properties
        public BillingAccount BillingAccount { get; set; }
        public Charge Charge { get; set; }
    }
}
