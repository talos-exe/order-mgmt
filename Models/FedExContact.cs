using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.Models
{
    public class FedExContact
    {
        [Required]
        public string PersonName { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}