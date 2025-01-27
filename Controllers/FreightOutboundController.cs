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
    public class FreightOutboundController : Controller
    {
        private readonly AppDbContext _context;

        public FreightOutboundController(AppDbContext context)
        {
            _context = context;
        }

        // GET: FreightOutbound
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.FreightOutbound.Include(f => f.User).Include(f => f.Warehouse);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Freight_Outbound_All()
        {
            var appDbContext = _context.FreightOutbound.Include(p => p.User).Include(p => p.Warehouse);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Freight_Outbound_Drafts()
        {
            var appDbContext = _context.FreightOutbound.Include(p => p.User).Include(p => p.Warehouse);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Freight_Outbound_Awaiting()
        {
            var appDbContext = _context.FreightOutbound.Include(p => p.User).Include(p => p.Warehouse);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Freight_Outbound_Completed()
        {
            var appDbContext = _context.FreightOutbound.Include(p => p.User).Include(p => p.Warehouse);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Freight_Outbound_Void()
        {
            var appDbContext = _context.FreightOutbound.Include(p => p.User).Include(p => p.Warehouse);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Freight_Outbound_Exceptions()
        {
            var appDbContext = _context.FreightOutbound.Include(p => p.User).Include(p => p.Warehouse);
            return View(await appDbContext.ToListAsync());
        }

        // GET: FreightOutbound/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var freightOutbound = await _context.FreightOutbound
                .Include(f => f.User)
                .Include(f => f.Warehouse)
                .FirstOrDefaultAsync(m => m.OutboundOrderId == id);
            if (freightOutbound == null)
            {
                return NotFound();
            }

            return View(freightOutbound);
        }

        // GET: FreightOutbound/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            ViewData["Warehouse_ID"] = new SelectList(_context.Warehouse, "Warehouse_ID", "Warehouse_ID");
            return View();
        }

        // POST: FreightOutbound/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OutboundOrderId,OrderType,OrderStatus,UserId,Warehouse_ID,ProductQuantity,CreationDate,EstimatedDeliveryDate,OrderShipDate,Cost,Currency,Recipient,RecipientPostCode,DestinationType,Platform,ShippingCompany,TransportDays,RelatedAdjustmentOrder,TrackingNumber,ReferenceOrderNumber,FBAShipmentId,FBATrackingNumber,OutboundMethod")] FreightOutbound freightOutbound)
        {
            if (ModelState.IsValid)
            {
                _context.Add(freightOutbound);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", freightOutbound.UserId);
            ViewData["Warehouse_ID"] = new SelectList(_context.Warehouse, "Warehouse_ID", "Warehouse_ID", freightOutbound.Warehouse_ID);
            return View(freightOutbound);
        }

        // GET: FreightOutbound/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var freightOutbound = await _context.FreightOutbound.FindAsync(id);
            if (freightOutbound == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", freightOutbound.UserId);
            ViewData["Warehouse_ID"] = new SelectList(_context.Warehouse, "Warehouse_ID", "Warehouse_ID", freightOutbound.Warehouse_ID);
            return View(freightOutbound);
        }

        // POST: FreightOutbound/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("OutboundOrderId,OrderType,OrderStatus,UserId,Warehouse_ID,ProductQuantity,CreationDate,EstimatedDeliveryDate,OrderShipDate,Cost,Currency,Recipient,RecipientPostCode,DestinationType,Platform,ShippingCompany,TransportDays,RelatedAdjustmentOrder,TrackingNumber,ReferenceOrderNumber,FBAShipmentId,FBATrackingNumber,OutboundMethod")] FreightOutbound freightOutbound)
        {
            if (id != freightOutbound.OutboundOrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(freightOutbound);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FreightOutboundExists(freightOutbound.OutboundOrderId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", freightOutbound.UserId);
            ViewData["Warehouse_ID"] = new SelectList(_context.Warehouse, "Warehouse_ID", "Warehouse_ID", freightOutbound.Warehouse_ID);
            return View(freightOutbound);
        }

        // GET: FreightOutbound/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var freightOutbound = await _context.FreightOutbound
                .Include(f => f.User)
                .Include(f => f.Warehouse)
                .FirstOrDefaultAsync(m => m.OutboundOrderId == id);
            if (freightOutbound == null)
            {
                return NotFound();
            }

            return View(freightOutbound);
        }

        // POST: FreightOutbound/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var freightOutbound = await _context.FreightOutbound.FindAsync(id);
            if (freightOutbound != null)
            {
                _context.FreightOutbound.Remove(freightOutbound);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FreightOutboundExists(string id)
        {
            return _context.FreightOutbound.Any(e => e.OutboundOrderId == id);
        }
    }
}
