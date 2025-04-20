using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace OrderMgmtRevision.Models
{

    public class Product
    {
        [Key]
        [BindNever]
        public string? ProductID { get; set; } // Product ID

        [Required (ErrorMessage = "Product name/title is required.")]
        [MaxLength(255)]
        [MinLength(4)]
        public string ProductName { get; set; } // Name for product

        [Required (ErrorMessage = "Product description is required.")]
        [MaxLength(255)]
        [MinLength(4)]
        public string Description {  get; set; } // Description of product

        [Required]
        [MaxLength(9)]
        [MinLength(9)]
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
        [Range(0, 999, ErrorMessage = "Stock must be between 0 and 999.")]
        public int Stock {  get; set; } = 0;

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow; // created date and updated date

        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        public string? CreatedBy { get; set; } = "System";

        public User? User { get; set; }

        public int? ShipAmount { get; set; } = 0;

        [Required(ErrorMessage = "Height (in) is required.")]
        [Range(0, 999, ErrorMessage = "Height must be between 0 and 999 inches.")]
        public int? Height { get; set; } = 0;

        [Required(ErrorMessage = "Width (in) is required.")]
        [Range(0, 999, ErrorMessage = "Width must be between 0 and 999 inches.")]
        public int? Width { get; set; } = 0;

        [Required(ErrorMessage = "Length (in) is required.")]
        [Range(0, 999, ErrorMessage = "Length must be between 0 and 999 inches.")]
        public int? Length { get; set; } = 0;

        [Required(ErrorMessage = "Weight (lbs) is required.")]
        [Range(0, 999, ErrorMessage = "Height must be between 0 and 150lbs.")]
        public int? Weight { get; set; } = 0;


    }
}
