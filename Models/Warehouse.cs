
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementSystem.Models
{
    public class Warehouse
    {
        [Key]
        [Column("Warehouse_ID")]
        [StringLength(25)]
        public string Warehouse_ID { get; set; }

        [Required]
        [Column("Warehouse")]
        [StringLength(50)]
        public string Name { get; set; }

        [Column("Country")]
        [StringLength(50)]
        public string Country { get; set; }

        [Column("City")]
        [StringLength(50)]
        public string City { get; set; }

        [Column("Currency")]
        [StringLength(50)]
        public string Currency { get; set; }

        // Navigation properties
        public ICollection<Inventory> Inventory { get; set; } = new List<Inventory>();
        public ICollection<InboundOrder> InboundOrder { get; set; } = new List<InboundOrder>();
        public ICollection<FreightOutbound> FreightOutbound { get; set; } = new List<FreightOutbound>();
        public ICollection<ParcelOutbound> ParcelOutbound { get; set; } = new List<ParcelOutbound>();
        public ICollection<PlatformOrder> PlatformOrder { get; set; } = new List<PlatformOrder>();
    }
}