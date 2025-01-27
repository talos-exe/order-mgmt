namespace OrderManagementSystem.Models
{
    public class RequestedPackageLineItem
    {
        public FedExWeight Weight { get; set; }
        public FedExDimensions Dimensions { get; set; }
    }
}
