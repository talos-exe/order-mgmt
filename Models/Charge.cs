
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementSystem.Models
{
    public class Charge
    {
        [Key]
        [Column("Charge_ID")]
        [StringLength(25)]
        public string ChargeId { get; set; }

        [Required]
        [Column("Amount")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be positive.")]
        public decimal Amount { get; set; }

        [Column("Charge_Type")]
        [StringLength(25)]
        public string ChargeType { get; set; }

        [Column("Description")]
        [StringLength(255)]
        public string Description { get; set; }

        // Navigation Properties
        public ICollection<Billing> Billing { get; set; } = new List<Billing>();
        
    }
}
