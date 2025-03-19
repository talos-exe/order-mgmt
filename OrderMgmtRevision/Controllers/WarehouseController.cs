using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderMgmtRevision.Models;

namespace OrderMgmtRevision.Controllers
{
    [Authorize]
    public class WarehouseController : Controller
    {
        public IActionResult Index()
        {
            var warehouseData = new List<Warehouse>
            {
              new Warehouse 
              {
                WarehouseID = 123,
                WarehouseName = "Arctic Warehouse",
                Address = "123 Test St"
              },
              new Warehouse 
              {
                WarehouseID = 456,
                WarehouseName = "Warehouse 2",
                Address = "456 Test St"
              }
            };

            return View(warehouseData);
        }
    }
}
