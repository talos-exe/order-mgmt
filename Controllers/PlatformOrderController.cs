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
    public class PlatformOrderController : Controller
    {
        private readonly AppDbContext _context;

        public PlatformOrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PlatformOrder
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.PlatformOrder.Include(p => p.User).Include(p => p.Warehouse);
            return View(await appDbContext.ToListAsync());
        }

        // GET: PlatformOrder/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platformOrder = await _context.PlatformOrder
                .Include(p => p.User)
                .Include(p => p.Warehouse)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (platformOrder == null)
            {
                return NotFound();
            }

            return View(platformOrder);
        }

        // GET: PlatformOrder/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            ViewData["Warehouse_ID"] = new SelectList(_context.Warehouse, "Warehouse_ID", "Warehouse_ID");
            return View();
        }

        // POST: PlatformOrder/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,Platform,Warehouse_ID,ProductQuantity,UserId,Buyer,RecipientPostcode,RecipientCountry,Store,Site,ShippingService,TrackingNumber,Carrier,OrderTime,PaymentTime,CreatedTime,OrderSource")] PlatformOrder platformOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(platformOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", platformOrder.UserId);
            ViewData["Warehouse_ID"] = new SelectList(_context.Warehouse, "Warehouse_ID", "Warehouse_ID", platformOrder.Warehouse_ID);
            return View(platformOrder);
        }

        // GET: PlatformOrder/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platformOrder = await _context.PlatformOrder.FindAsync(id);
            if (platformOrder == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", platformOrder.UserId);
            ViewData["Warehouse_ID"] = new SelectList(_context.Warehouse, "Warehouse_ID", "Warehouse_ID", platformOrder.Warehouse_ID);
            return View(platformOrder);
        }

        // POST: PlatformOrder/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("OrderId,Platform,Warehouse_ID,ProductQuantity,UserId,Buyer,RecipientPostcode,RecipientCountry,Store,Site,ShippingService,TrackingNumber,Carrier,OrderTime,PaymentTime,CreatedTime,OrderSource")] PlatformOrder platformOrder)
        {
            if (id != platformOrder.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(platformOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlatformOrderExists(platformOrder.OrderId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", platformOrder.UserId);
            ViewData["Warehouse_ID"] = new SelectList(_context.Warehouse, "Warehouse_ID", "Warehouse_ID", platformOrder.Warehouse_ID);
            return View(platformOrder);
        }

        // GET: PlatformOrder/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platformOrder = await _context.PlatformOrder
                .Include(p => p.User)
                .Include(p => p.Warehouse)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (platformOrder == null)
            {
                return NotFound();
            }

            return View(platformOrder);
        }

        // POST: PlatformOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var platformOrder = await _context.PlatformOrder.FindAsync(id);
            if (platformOrder != null)
            {
                _context.PlatformOrder.Remove(platformOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlatformOrderExists(string id)
        {
            return _context.PlatformOrder.Any(e => e.OrderId == id);
        }
    }
}
