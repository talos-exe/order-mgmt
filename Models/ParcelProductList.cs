
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementSystem.Models
{
    public class ParcelProductList
    {
        [StringLength(25)]
        public string OrderId { get; set; }

        [StringLength(25)]
        public string ProductId { get; set; }

        public int Quantity { get; set; }

        // Navigation Properties
        public ParcelOutbound ParcelOutbound { get; set; }
        public Inventory Inventory { get; set; }
    }
}
