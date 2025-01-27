
using OrderManagementSystem.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementSystem.Models
{
    public class BillingAccount
    {
        [Key]
        [Column("Billing_Account_ID")]
        [StringLength(25)]
        public string BillingAccountId { get; set; }

        [Required]
        [Column("User_ID")]
        [StringLength(25)]
        public string UserId { get; set; }

        [Column("Account_Balance", TypeName = "decimal(10, 2)")]
        public decimal AccountBalance { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public ICollection<Billing> Billing { get; set; } = new List<Billing>();
    }
}
