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
    public class FreightProductListController : Controller
    {
        private readonly AppDbContext _context;

        public FreightProductListController(AppDbContext context)
        {
            _context = context;
        }

        // GET: FreightProductList
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.FreightProductList.Include(f => f.FreightOutbound).Include(f => f.Inventory);
            return View(await appDbContext.ToListAsync());
        }

        // GET: FreightProductList/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var freightProductList = await _context.FreightProductList
                .Include(f => f.FreightOutbound)
                .Include(f => f.Inventory)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (freightProductList == null)
            {
                return NotFound();
            }

            return View(freightProductList);
        }

        // GET: FreightProductList/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.FreightOutbound, "OutboundOrderId", "OutboundOrderId");
            ViewData["ProductId"] = new SelectList(_context.Inventory, "ProductId", "ProductId");
            return View();
        }

        // POST: FreightProductList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,ProductId,Quantity")] FreightProductList freightProductList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(freightProductList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.FreightOutbound, "OutboundOrderId", "OutboundOrderId", freightProductList.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Inventory, "ProductId", "ProductId", freightProductList.ProductId);
            return View(freightProductList);
        }

        // GET: FreightProductList/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var freightProductList = await _context.FreightProductList.FindAsync(id);
            if (freightProductList == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.FreightOutbound, "OutboundOrderId", "OutboundOrderId", freightProductList.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Inventory, "ProductId", "ProductId", freightProductList.ProductId);
            return View(freightProductList);
        }

        // POST: FreightProductList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("OrderId,ProductId,Quantity")] FreightProductList freightProductList)
        {
            if (id != freightProductList.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(freightProductList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FreightProductListExists(freightProductList.OrderId))
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
            ViewData["OrderId"] = new SelectList(_context.FreightOutbound, "OutboundOrderId", "OutboundOrderId", freightProductList.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Inventory, "ProductId", "ProductId", freightProductList.ProductId);
            return View(freightProductList);
        }

        // GET: FreightProductList/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var freightProductList = await _context.FreightProductList
                .Include(f => f.FreightOutbound)
                .Include(f => f.Inventory)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (freightProductList == null)
            {
                return NotFound();
            }

            return View(freightProductList);
        }

        // POST: FreightProductList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var freightProductList = await _context.FreightProductList.FindAsync(id);
            if (freightProductList != null)
            {
                _context.FreightProductList.Remove(freightProductList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FreightProductListExists(string id)
        {
            return _context.FreightProductList.Any(e => e.OrderId == id);
        }
    }
}
