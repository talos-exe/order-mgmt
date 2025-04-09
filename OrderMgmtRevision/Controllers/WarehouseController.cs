using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OrderMgmtRevision.Data;
using OrderMgmtRevision.Models;

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
            try
            {
              var warehouses = await _context.Warehouses.ToListAsync();
              return View(warehouses);
            }
            catch (Exception ex)
            {
              return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        public async Task<IActionResult> CreateConfirm(Warehouse model)
        {
          var warehouse = new Warehouse
          {
            WarehouseName = model.WarehouseName,
            Address = model.Address,
            CreatedAt = DateTime.Now
          };
          
          _context.Warehouses.Add(model);
          return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteConfirm(int id)
        {
          try
            {
                var warehouse = await _context.Warehouses.FindAsync(id);
                _context.Warehouses.Remove(warehouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting warehouse: {ex.Message}");
            }
        }
    }
}
