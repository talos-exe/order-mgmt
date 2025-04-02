namespace OrderMgmtRevision.Models
{
    public class Inventory
    {
        public int InventoryID { get; set; }

        public string ProductID { get; set; }

        public int WarehouseID { get; set; }

        public int Quantity { get; set; } = 0;

        public Product Product { get; set; }
        public Warehouse Warehouse { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
