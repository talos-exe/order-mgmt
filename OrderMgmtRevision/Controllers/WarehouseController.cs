using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderMgmtRevision.Data;
using OrderMgmtRevision.Models;
using OrderMgmtRevision.Services;
using Stripe;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OrderMgmtRevision.Controllers
{
    [Authorize]
    public class WarehouseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogService _logService;
        public WarehouseController(AppDbContext context, UserManager<User> userManager, ILogService logService)
        {
            _context = context;
            _userManager = userManager;
            _logService = logService;
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

        private async Task<bool> IsUserAdmin()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return false;
            }

            var roles = await _userManager.GetRolesAsync(user);
            return roles.Contains("Admin");
        }

        private string GetClientIp()
        {
            return HttpContext.Connection.RemoteIpAddress?.ToString();
        }


        [HttpPost]
        public async Task<IActionResult> CreateWorkOrder(WorkOrder workOrder)
        {
            var user = await _userManager.GetUserAsync(User);
            string userName = user?.UserName ?? "Unknown";
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

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

                if (double.IsNaN(workOrder.Fee))
                {
                    ModelState.AddModelError("Fee", "Fee cannot be text.");
                }

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
            workOrder.CreatedBy = userName;

            _context.WorkOrders.Add(workOrder);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "New workorder has been created.";
            await _logService.LogUserActivityAsync(user.Id, $"New workorder {workOrder.Id} has been created.", GetClientIp());

            return RedirectToAction("ViewWorkOrders", new { warehouseId = workOrder.WarehouseId });
        }

        public async Task<IActionResult> ViewWorkOrders(int warehouseId)
        {
            var user = await _userManager.GetUserAsync(User);
            string userName = user?.UserName ?? "Unknown";
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            var workOrders = await _context.WorkOrders
                .Include(w => w.Warehouse)
                .Where(w => w.WarehouseId == warehouseId && w.CreatedBy == user.UserName)
                .ToListAsync();

            ViewBag.WarehouseName = (await _context.Warehouses.FindAsync(warehouseId))?.WarehouseName;
            return View(workOrders);
        }

        [HttpGet]
        public async Task<IActionResult> GetWarehouseDetails(int warehouseId)
        {
            var warehouse = await _context.Warehouses
                .Where(w => w.WarehouseID == warehouseId)
                .Select(w => new
                {
                    address = w.Address,
                    city = w.City,
                    state = w.State,
                    zip = w.Zip,
                    country = w.CountryCode,
                    phone = w.PhoneNumber,
                    email = w.WarehouseEmail,
                    name = w.WarehouseName
                })
                .FirstOrDefaultAsync();

            if (warehouse == null)
            {
                return NotFound();
            }

            return Json(warehouse);
        }
    }
}
