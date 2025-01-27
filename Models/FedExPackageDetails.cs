namespace OrderManagementSystem.Models
{
    public class FedExPackageDetails
    {
        public  int Weight { get; set; }
        
        public FedExDimensions Dimensions { get; set; }
    }
}
