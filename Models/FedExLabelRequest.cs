using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.Models
{
    public class FedExLabelRequest
    {
        [Required]
        public FedExAddress OriginAddress { get; set; }

        [Required]
        public FedExAddress DestinationAddress { get; set; }

        [Required]
        public FedExPackageDetails PackageDetails { get; set; }

        [Required]
        public string ServiceType { get; set; }

        [Required]
        public string PackagingType { get; set; }

        [Required]
        public string PickupType { get; set; }

        [Required]
        public string ShipDateStamp { get; set; }

        [Required]
        public string AccountNumber { get; set; }
    }
}