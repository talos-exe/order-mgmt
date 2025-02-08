using System.ComponentModel.DataAnnotations;

namespace OrderMgmtRevision.Models
{
    public class Warehouse
    {
        public int WarehouseID { get; set; }

        [Required]
        [MaxLength(100)]
        public string WarehouseName { get; set; }

        public string Address { get; set; } = "Unknown";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
