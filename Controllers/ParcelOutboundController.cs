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
    public class ParcelOutboundController : Controller
    {
        private readonly AppDbContext _context;

        public ParcelOutboundController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ParcelOutbound
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.ParcelOutbound.Include(p => p.User).Include(p => p.Warehouse);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Parcel_Outbound_All()
        {
            var appDbContext = _context.ParcelOutbound.Include(p => p.User).Include(p => p.Warehouse);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Parcel_Outbound_Drafts()
        {
            var appDbContext = _context.ParcelOutbound.Include(p => p.User).Include(p => p.Warehouse);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Parcel_Outbound_Awaiting()
        {
            var appDbContext = _context.ParcelOutbound.Include(p => p.User).Include(p => p.Warehouse);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Parcel_Outbound_Completed()
        {
            var appDbContext = _context.ParcelOutbound.Include(p => p.User).Include(p => p.Warehouse);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Parcel_Outbound_Void()
        {
            var appDbContext = _context.ParcelOutbound.Include(p => p.User).Include(p => p.Warehouse);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Parcel_Outbound_Exceptions()
        {
            var appDbContext = _context.ParcelOutbound.Include(p => p.User).Include(p => p.Warehouse);
            return View(await appDbContext.ToListAsync());
        }

        // GET: ParcelOutbound/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcelOutbound = await _context.ParcelOutbound
                .Include(p => p.User)
                .Include(p => p.Warehouse)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (parcelOutbound == null)
            {
                return NotFound();
            }

            return View(parcelOutbound);
        }

        // GET: ParcelOutbound/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            ViewData["Warehouse_ID"] = new SelectList(_context.Warehouse, "Warehouse_ID", "Warehouse_ID");
            return View();
        }

        // POST: ParcelOutbound/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,OrderType,OrderStatus,Warehouse_ID,UserId,Platform,EstimatedDeliveryDate,ShipDate,TransportDays,Cost,Currency,Recipient,Country,Postcode,TrackingNumber,ReferenceOrderNumber,CreationDate,Boxes,ShippingCompany,LatestInformation,TrackingUpdateTime,InternetPostingTime,DeliveryTime,RelatedAdjustmentOrder")] ParcelOutbound parcelOutbound)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parcelOutbound);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", parcelOutbound.UserId);
            ViewData["Warehouse_ID"] = new SelectList(_context.Warehouse, "Warehouse_ID", "Warehouse_ID", parcelOutbound.Warehouse_ID);
            return View(parcelOutbound);
        }

        // GET: ParcelOutbound/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcelOutbound = await _context.ParcelOutbound.FindAsync(id);
            if (parcelOutbound == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", parcelOutbound.UserId);
            ViewData["Warehouse_ID"] = new SelectList(_context.Warehouse, "Warehouse_ID", "Warehouse_ID", parcelOutbound.Warehouse_ID);
            return View(parcelOutbound);
        }

        // POST: ParcelOutbound/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("OrderId,OrderType,OrderStatus,Warehouse_ID,UserId,Platform,EstimatedDeliveryDate,ShipDate,TransportDays,Cost,Currency,Recipient,Country,Postcode,TrackingNumber,ReferenceOrderNumber,CreationDate,Boxes,ShippingCompany,LatestInformation,TrackingUpdateTime,InternetPostingTime,DeliveryTime,RelatedAdjustmentOrder")] ParcelOutbound parcelOutbound)
        {
            if (id != parcelOutbound.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parcelOutbound);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParcelOutboundExists(parcelOutbound.OrderId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", parcelOutbound.UserId);
            ViewData["Warehouse_ID"] = new SelectList(_context.Warehouse, "Warehouse_ID", "Warehouse_ID", parcelOutbound.Warehouse_ID);
            return View(parcelOutbound);
        }

        // GET: ParcelOutbound/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcelOutbound = await _context.ParcelOutbound
                .Include(p => p.User)
                .Include(p => p.Warehouse)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (parcelOutbound == null)
            {
                return NotFound();
            }

            return View(parcelOutbound);
        }

        // POST: ParcelOutbound/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var parcelOutbound = await _context.ParcelOutbound.FindAsync(id);
            if (parcelOutbound != null)
            {
                _context.ParcelOutbound.Remove(parcelOutbound);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParcelOutboundExists(string id)
        {
            return _context.ParcelOutbound.Any(e => e.OrderId == id);
        }
    }
}
