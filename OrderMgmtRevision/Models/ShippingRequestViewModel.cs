using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OrderMgmtRevision.Models
{
    public class ShippingRequestViewModel : ShippingRequest
    {
        public int SourceWarehouseId { get; set; }
        public string ProductID { get; set; }

        [BindNever]
        public List<Product> ProductList { get; set; }

        [BindNever]
        public List<Warehouse> Warehouses { get; set; }
    }
}
