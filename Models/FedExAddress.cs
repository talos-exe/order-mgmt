using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.Models
{
    public class FedExAddress
    {
        [Required]
        public List<string> StreetLines { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string StateOrProvinceCode { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string CountryCode { get; set; }

        [Required]
        public FedExContact Contact { get; set; }
    }
}