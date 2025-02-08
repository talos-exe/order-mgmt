using System.ComponentModel.DataAnnotations;

namespace OrderMgmtRevision.Models
{

    public class Product
    {
        public int ProductID { get; set; } // Product ID

        [Required]
        [MaxLength(255)]
        public string ProductName { get; set; } // Name for product

        public string Description {  get; set; } // Description of product

        [Required]
        [MaxLength(255)]
        public string SKU { get; set; } // stock keeping unit

        [Required]
        public decimal Price { get; set; } = 0; // product price listing for outbound calculation

        [Required]
        public decimal Cost { get; set; } = 0;// product cost for inbound calculation, 0 if null

        public DateTime CreatedAt { get; set; } = DateTime.Now; // created date and updated date

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}
