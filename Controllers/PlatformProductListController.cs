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
    public class PlatformProductListController : Controller
    {
        private readonly AppDbContext _context;

        public PlatformProductListController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PlatformProductList
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.PlatformProductList.Include(p => p.Inventory).Include(p => p.PlatformOrder);
            return View(await appDbContext.ToListAsync());
        }

        // GET: PlatformProductList/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platformProductList = await _context.PlatformProductList
                .Include(p => p.Inventory)
                .Include(p => p.PlatformOrder)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (platformProductList == null)
            {
                return NotFound();
            }

            return View(platformProductList);
        }

        // GET: PlatformProductList/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Inventory, "ProductId", "ProductId");
            ViewData["OrderId"] = new SelectList(_context.PlatformOrder, "OrderId", "OrderId");
            return View();
        }

        // POST: PlatformProductList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,ProductId,Quantity")] PlatformProductList platformProductList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(platformProductList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Inventory, "ProductId", "ProductId", platformProductList.ProductId);
            ViewData["OrderId"] = new SelectList(_context.PlatformOrder, "OrderId", "OrderId", platformProductList.OrderId);
            return View(platformProductList);
        }

        // GET: PlatformProductList/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platformProductList = await _context.PlatformProductList.FindAsync(id);
            if (platformProductList == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Inventory, "ProductId", "ProductId", platformProductList.ProductId);
            ViewData["OrderId"] = new SelectList(_context.PlatformOrder, "OrderId", "OrderId", platformProductList.OrderId);
            return View(platformProductList);
        }

        // POST: PlatformProductList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("OrderId,ProductId,Quantity")] PlatformProductList platformProductList)
        {
            if (id != platformProductList.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(platformProductList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlatformProductListExists(platformProductList.OrderId))
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
            ViewData["ProductId"] = new SelectList(_context.Inventory, "ProductId", "ProductId", platformProductList.ProductId);
            ViewData["OrderId"] = new SelectList(_context.PlatformOrder, "OrderId", "OrderId", platformProductList.OrderId);
            return View(platformProductList);
        }

        // GET: PlatformProductList/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platformProductList = await _context.PlatformProductList
                .Include(p => p.Inventory)
                .Include(p => p.PlatformOrder)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (platformProductList == null)
            {
                return NotFound();
            }

            return View(platformProductList);
        }

        // POST: PlatformProductList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var platformProductList = await _context.PlatformProductList.FindAsync(id);
            if (platformProductList != null)
            {
                _context.PlatformProductList.Remove(platformProductList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlatformProductListExists(string id)
        {
            return _context.PlatformProductList.Any(e => e.OrderId == id);
        }
    }
}
