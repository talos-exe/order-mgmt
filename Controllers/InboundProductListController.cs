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
    public class InboundProductListController : Controller
    {
        private readonly AppDbContext _context;

        public InboundProductListController(AppDbContext context)
        {
            _context = context;
        }

        // GET: InboundProductList
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.InboundProductList.Include(i => i.InboundOrder).Include(i => i.Inventory);
            return View(await appDbContext.ToListAsync());
        }

        // GET: InboundProductList/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inboundProductList = await _context.InboundProductList
                .Include(i => i.InboundOrder)
                .Include(i => i.Inventory)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (inboundProductList == null)
            {
                return NotFound();
            }

            return View(inboundProductList);
        }

        // GET: InboundProductList/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.InboundOrder, "InboundOrderId", "InboundOrderId");
            ViewData["ProductId"] = new SelectList(_context.Inventory, "ProductId", "ProductId");
            return View();
        }

        // POST: InboundProductList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,ProductId,Quantity")] InboundProductList inboundProductList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inboundProductList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.InboundOrder, "InboundOrderId", "InboundOrderId", inboundProductList.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Inventory, "ProductId", "ProductId", inboundProductList.ProductId);
            return View(inboundProductList);
        }

        // GET: InboundProductList/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inboundProductList = await _context.InboundProductList.FindAsync(id);
            if (inboundProductList == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.InboundOrder, "InboundOrderId", "InboundOrderId", inboundProductList.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Inventory, "ProductId", "ProductId", inboundProductList.ProductId);
            return View(inboundProductList);
        }

        // POST: InboundProductList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("OrderId,ProductId,Quantity")] InboundProductList inboundProductList)
        {
            if (id != inboundProductList.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inboundProductList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InboundProductListExists(inboundProductList.OrderId))
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
            ViewData["OrderId"] = new SelectList(_context.InboundOrder, "InboundOrderId", "InboundOrderId", inboundProductList.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Inventory, "ProductId", "ProductId", inboundProductList.ProductId);
            return View(inboundProductList);
        }

        // GET: InboundProductList/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inboundProductList = await _context.InboundProductList
                .Include(i => i.InboundOrder)
                .Include(i => i.Inventory)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (inboundProductList == null)
            {
                return NotFound();
            }

            return View(inboundProductList);
        }

        // POST: InboundProductList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var inboundProductList = await _context.InboundProductList.FindAsync(id);
            if (inboundProductList != null)
            {
                _context.InboundProductList.Remove(inboundProductList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InboundProductListExists(string id)
        {
            return _context.InboundProductList.Any(e => e.OrderId == id);
        }
    }
}
