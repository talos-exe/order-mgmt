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

<<<<<<< Updated upstream
            return View(warehouseData);
=======
        public WarehouseController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var warehouses = await _context.Warehouses.ToListAsync();
            return View(warehouses);
        }

        public async Task<IActionResult> CreateConfirm(Warehouse model)
        {
            var warehouse = new Warehouse
            {
                WarehouseName = model.WarehouseName,
                Address = model.Address,
                CreatedAt = DateTime.Now
            };

            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            _context.Warehouses.Remove(warehouse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult CreateWorkOrder(int warehouseId)
        {
            ViewBag.WarehouseId = warehouseId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkOrder(WorkOrder workOrder)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.WarehouseId = workOrder.WarehouseId;
                return View(workOrder);
            }

            _context.WorkOrders.Add(workOrder);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewWorkOrders", new { warehouseId = workOrder.WarehouseId });
        }

        public async Task<IActionResult> ViewWorkOrders(int warehouseId)
        {
            var workOrders = await _context.WorkOrders
                .Include(w => w.Warehouse)
                .Where(w => w.WarehouseId == warehouseId)
                .ToListAsync();

            ViewBag.WarehouseName = (await _context.Warehouses.FindAsync(warehouseId))?.WarehouseName;
            return View(workOrders);
>>>>>>> Stashed changes
        }
    }
}
