using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderMgmtRevision.Data;
using OrderMgmtRevision.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OrderMgmtRevision.Controllers
{
    [Authorize(Roles = "Admin")]
    public class WarehouseController : Controller
    {
        private readonly AppDbContext _context;
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
            ViewBag.WarehouseId = workOrder.WarehouseId;
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                 .Where(x => x.Value.Errors.Count > 0)
                 .Select(x => new {
                     Property = x.Key,
                     ErrorMessages = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                 })
                 .ToList();

                // Create a list to store formatted error messages
                var errorMessages = new List<string>();

                // Format each error message
                foreach (var error in errors)
                {
                    foreach (var message in error.ErrorMessages)
                    {
                        // Add property name if available
                        if (!string.IsNullOrEmpty(error.Property))
                            errorMessages.Add($"{error.Property}: {message}");
                        else
                            errorMessages.Add(message);
                    }
                }

                // Store the error messages in TempData
                TempData["Errors"] = errorMessages;
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
        }
    }
}
