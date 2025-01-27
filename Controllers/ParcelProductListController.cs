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
    public class ParcelProductListController : Controller
    {
        private readonly AppDbContext _context;

        public ParcelProductListController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ParcelProductList
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.ParcelProductList.Include(p => p.Inventory).Include(p => p.ParcelOutbound);
            return View(await appDbContext.ToListAsync());
        }

        // GET: ParcelProductList/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcelProductList = await _context.ParcelProductList
                .Include(p => p.Inventory)
                .Include(p => p.ParcelOutbound)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (parcelProductList == null)
            {
                return NotFound();
            }

            return View(parcelProductList);
        }

        // GET: ParcelProductList/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Inventory, "ProductId", "ProductId");
            ViewData["OrderId"] = new SelectList(_context.ParcelOutbound, "OrderId", "OrderId");
            return View();
        }

        // POST: ParcelProductList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,ProductId,Quantity")] ParcelProductList parcelProductList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parcelProductList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Inventory, "ProductId", "ProductId", parcelProductList.ProductId);
            ViewData["OrderId"] = new SelectList(_context.ParcelOutbound, "OrderId", "OrderId", parcelProductList.OrderId);
            return View(parcelProductList);
        }

        // GET: ParcelProductList/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcelProductList = await _context.ParcelProductList.FindAsync(id);
            if (parcelProductList == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Inventory, "ProductId", "ProductId", parcelProductList.ProductId);
            ViewData["OrderId"] = new SelectList(_context.ParcelOutbound, "OrderId", "OrderId", parcelProductList.OrderId);
            return View(parcelProductList);
        }

        // POST: ParcelProductList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("OrderId,ProductId,Quantity")] ParcelProductList parcelProductList)
        {
            if (id != parcelProductList.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parcelProductList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParcelProductListExists(parcelProductList.OrderId))
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
            ViewData["ProductId"] = new SelectList(_context.Inventory, "ProductId", "ProductId", parcelProductList.ProductId);
            ViewData["OrderId"] = new SelectList(_context.ParcelOutbound, "OrderId", "OrderId", parcelProductList.OrderId);
            return View(parcelProductList);
        }

        // GET: ParcelProductList/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcelProductList = await _context.ParcelProductList
                .Include(p => p.Inventory)
                .Include(p => p.ParcelOutbound)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (parcelProductList == null)
            {
                return NotFound();
            }

            return View(parcelProductList);
        }

        // POST: ParcelProductList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var parcelProductList = await _context.ParcelProductList.FindAsync(id);
            if (parcelProductList != null)
            {
                _context.ParcelProductList.Remove(parcelProductList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParcelProductListExists(string id)
        {
            return _context.ParcelProductList.Any(e => e.OrderId == id);
        }
    }
}
