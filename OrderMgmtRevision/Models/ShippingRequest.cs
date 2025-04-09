using System.ComponentModel.DataAnnotations;

namespace OrderMgmtRevision.Models
{
    public class ShippingRequest
    {
        [Required]
        public string FromName { get; set; }
        [Required]
        public string FromStreet { get; set; }
        [Required]
        public string FromCity { get; set; }
        [Required]
        public string FromState { get; set; }

        [Required]
        public string FromZip { get; set; }

        [Required]
        public string ToName { get; set; }
        [Required]
        public string ToStreet { get; set; }
        [Required]
        public string ToCity { get; set; }
        [Required]
        public string ToState { get; set; }
        [Required]
        public string ToZip { get; set; }

        [Required]
        [Range(0.1, 100)]
        public decimal Weight { get; set; }

        [Required]
        [Range(0.1, 100)]
        public decimal Length { get; set; }

        [Required]
        [Range(0.1, 100)]
        public decimal Width { get; set; }

        [Required]
        [Range(0.1, 100)]
        public decimal Height { get; set; }
    }
}
