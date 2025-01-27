
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementSystem.Models
{
    public class FreightProductList
    {
        [StringLength(25)]
        public string OrderId { get; set; }

        [StringLength(25)]
        public string ProductId { get; set; }

        public int Quantity { get; set; }

        // Navigation Properties
        public FreightOutbound FreightOutbound { get; set; }
        public Inventory Inventory { get; set; }
    }
}
