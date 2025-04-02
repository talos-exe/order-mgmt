using System.ComponentModel.DataAnnotations;

namespace OrderMgmtRevision.Models
{

    public class Product
    {
        [Key]
        public string ProductID { get; set; } // Product ID

        [Required (ErrorMessage = "Product name/title is required.")]
        [MaxLength(255)]
        public string ProductName { get; set; } // Name for product

        [MaxLength(255)]
        public string Description {  get; set; } // Description of product

        [Required]
        [MaxLength(9)]
        [RegularExpression("^[A-Za-z0-9]{4}-[A-Za-z0-9]{4}$", ErrorMessage = "SKU must be in the format xxxx-xxxx.")]
        public string SKU { get; set; } // stock keeping unit

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        [DisplayFormat(DataFormatString = "{0:C4}")]
        public decimal Price { get; set; } = 0; // product price listing for outbound calculation

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Cost must be a positive value.")]
        [DisplayFormat(DataFormatString = "{0:C4}")]
        public decimal Cost { get; set; } = 0;// product cost for inbound calculation, 0 if null

        [Required]
        [Range(0, 9999, ErrorMessage = "Stock must be between 0 and 999.")]
        public int Stock {  get; set; } = 0;

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow; // created date and updated date

        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        public string? CreatedBy { get; set; } = "System";

        public User User { get; set; }

    }
}
