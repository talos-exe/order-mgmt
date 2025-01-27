using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Controllers
{
    public class InventoryController : Controller
    {
        private readonly AppDbContext _context;

        public InventoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Inventory
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Inventory.Include(i => i.Warehouse);
            return View(await appDbContext.ToListAsync());
            //return View(await _context.Inventory.ToListAsync());
        }

        public async Task<IActionResult> Products_All()
        {
            var appDbContext = _context.Inventory.Include(i => i.Warehouse);
            return View(await appDbContext.ToListAsync());
            //return View(await _context.Inventory.ToListAsync());
        }

        public async Task<IActionResult> Products_Discard()
        {
            var appDbContext = _context.Inventory.Include(i => i.Warehouse);
            return View(await appDbContext.ToListAsync());
            //return View(await _context.Inventory.ToListAsync());
        }

        public async Task<IActionResult> Products_Rejected()
        {
            var appDbContext = _context.Inventory.Include(i => i.Warehouse);
            return View(await appDbContext.ToListAsync());
            //return View(await _context.Inventory.ToListAsync());
        }

        public async Task<IActionResult> Products_Reviewed()
        {
            var appDbContext = _context.Inventory.Include(i => i.Warehouse);
            return View(await appDbContext.ToListAsync());
            //return View(await _context.Inventory.ToListAsync());
        }

        public async Task<IActionResult> Products_Under_Review()
        {
            var appDbContext = _context.Inventory.Include(i => i.Warehouse);
            return View(await appDbContext.ToListAsync());
            //return View(await _context.Inventory.ToListAsync());
        }

        public async Task<IActionResult> Products()
        {
            var appDbContext = _context.Inventory.Include(i => i.Warehouse);
            return View(await appDbContext.ToListAsync());
            //return View(await _context.Inventory.ToListAsync());
        }

        // GET: Inventory/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(m => m.Product_ID == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: Inventory/Create
        public IActionResult Create()
        {
            ViewData["Warehouse_ID"] = new SelectList(_context.Warehouse, "Warehouse_ID", "Warehouse_ID");
            return View();
        }

        // POST: Inventory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Product_ID,Warehouse_ID,SKU,Product_Name,Product_Description,Price,Quantity")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Warehouse_ID"] = new SelectList(_context.Warehouse, "Warehouse_ID", "Warehouse_ID", inventory.Warehouse_ID);
            return View(inventory);
        }

        // GET: Inventory/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }
            ViewData["Warehouse_ID"] = new SelectList(_context.Warehouse, "Warehouse_ID", "Warehouse_ID", inventory.Warehouse_ID);
            return View(inventory);
        }

        // POST: Inventory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Product_ID,Warehouse_ID,SKU,Product_Name,Product_Description,Price,Quantity")] Inventory inventory)
        {
            if (id != inventory.Product_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.Product_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Warehouse_ID"] = new SelectList(_context.Warehouse, "Warehouse_ID", "Warehouse_ID", inventory.Warehouse_ID);
            return View(inventory);
        }

        // GET: Inventory/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(m => m.Product_ID == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // POST: Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory != null)
            {
                _context.Inventory.Remove(inventory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(string id)
        {
            return _context.Inventory.Any(e => e.Product_ID == id);
        }
    }
}
