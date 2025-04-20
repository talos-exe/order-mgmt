using System.ComponentModel.DataAnnotations;

namespace OrderMgmtRevision.Models
{
    public class Warehouse
    {
        public int WarehouseID { get; set; }

        [Required]
        [MaxLength(100)]
        public string WarehouseName { get; set; }

        public string City { get; set; } = "Unknown";
        public string Address { get; set; } = "Unknown";

        public string State { get; set; } = "Unknown";
        public string Zip {  get; set; } = "Unknown";
        public string PhoneNumber {  get; set; } = "111-222-3333";
        public string CountryCode { get; set; } = "US";
        public string WarehouseEmail { get; set; } = "warehouse@gmail.com";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
