
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementSystem.Models
{
    public class User
    {
        [Key]
        [Column("User_ID")]
        [StringLength(25)]
        public string UserId { get; set; }

        [Required]
        [StringLength(25)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Column("Date_Created")]
        public DateTime DateCreated { get; set; }

        // Navigation properties
        public ICollection<BillingAccount> BillingAccount { get; set; } = new List<BillingAccount>();
        public ICollection<UsersRole> UserRole { get; set; } = new List<UsersRole>();
        public ICollection<Order> Order { get; set; } = new List<Order>();
        public ICollection<FreightOutbound> FreightOutbound { get; set; } = new List<FreightOutbound>();
        public ICollection<InboundOrder> InboundOrder { get; set; } = new List<InboundOrder>();
        public ICollection<ParcelOutbound> ParcelOutbound { get; set; } = new List<ParcelOutbound>();
        public ICollection<PlatformOrder> PlatformOrder { get; set; } = new List<PlatformOrder>();
    }
}