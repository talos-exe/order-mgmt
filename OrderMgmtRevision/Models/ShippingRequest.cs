using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace OrderMgmtRevision.Models
{
    [Owned]
    public class ShippingRequest
    {
        public string FromName { get; set; }

        public string FromStreet { get; set; }

        public string FromCity { get; set; }

        public string FromState { get; set; }

        public string FromZip { get; set; }

        public string FromPhone {  get; set; }

        public string FromEmail { get; set; }

        [Required(ErrorMessage = "A recipient name is required.")]
        public string ToName { get; set; }
        [Required(ErrorMessage = "A recipient street is required.")]
        public string ToStreet { get; set; }
        [Required(ErrorMessage = "A recipient city is required.")]
        public string ToCity { get; set; }
        [Required(ErrorMessage = "A state is required.")]
        public string ToState { get; set; }
        [Required(ErrorMessage = "A zip code is required.")]
        public string ToZip { get; set; }

        [Required(ErrorMessage = "A country code is required.")]
        public string ToCountryCode { get; set; } = "US";

        public string? ToPhone { get; set; } = "Not Given";

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
