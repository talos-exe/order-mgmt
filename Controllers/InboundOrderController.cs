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
    public class InboundOrderController : Controller
    {
        private readonly AppDbContext _context;

        public InboundOrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: InboundOrder
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.InboundOrder.Include(i => i.User).Include(i => i.Warehouse);
            return View(await appDbContext.ToListAsync());
            //return View(await _context.InboundOrder.ToListAsync());
        }

        public async Task<IActionResult> Inbound_All()
        {
            var appDbContext = _context.InboundOrder.Include(i => i.User).Include(i => i.Warehouse);
            return View(await appDbContext.ToListAsync());
            //return View(await _context.InboundOrder.ToListAsync());
        }

        public async Task<IActionResult> Inbound_Awaiting()
        {
            var appDbContext = _context.InboundOrder.Include(i => i.User).Include(i => i.Warehouse);
            return View(await appDbContext.ToListAsync());
            //return View(await _context.InboundOrder.ToListAsync());
        }

        public async Task<IActionResult> Inbound_Drafts()
        {
            var appDbContext = _context.InboundOrder.Include(i => i.User).Include(i => i.Warehouse);
            return View(await appDbContext.ToListAsync());
            //return View(await _context.InboundOrder.ToListAsync());
        }

        public async Task<IActionResult> Inbound_Recieved()
        {
            var appDbContext = _context.InboundOrder.Include(i => i.User).Include(i => i.Warehouse);
            return View(await appDbContext.ToListAsync());
            //return View(await _context.InboundOrder.ToListAsync());
        }

        public async Task<IActionResult> Inbound_Recieving()
        {
            var appDbContext = _context.InboundOrder.Include(i => i.User).Include(i => i.Warehouse);
            return View(await appDbContext.ToListAsync());
            //return View(await _context.InboundOrder.ToListAsync());
        }

        public async Task<IActionResult> Inbound_Shelved()
        {
            var appDbContext = _context.InboundOrder.Include(i => i.User).Include(i => i.Warehouse);
            return View(await appDbContext.ToListAsync());
            //return View(await _context.InboundOrder.ToListAsync());
        }

        public async Task<IActionResult> Inbound_Void()
        {
            var appDbContext = _context.InboundOrder.Include(i => i.User).Include(i => i.Warehouse);
            return View(await appDbContext.ToListAsync());
            //return View(await _context.InboundOrder.ToListAsync());
        }

        // GET: InboundOrder/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inboundOrder = await _context.InboundOrder
                .Include(i => i.User)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(m => m.InboundOrderId == id);
            if (inboundOrder == null)
            {
                return NotFound();
            }

            return View(inboundOrder);
        }

        // GET: InboundOrder/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            ViewData["Warehouse_ID"] = new SelectList(_context.Warehouse, "Warehouse_ID", "Warehouse_ID");
            return View();
        }

        // POST: InboundOrder/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InboundOrderId,OrderStatus,UserId,Warehouse_ID,EstimatedArrival,ProductQuantity,CreationDate,Cost,Currency,Boxes,InboundType,TrackingNumber,ReferenceOrderNumber,ArrivalMethod")] InboundOrder inboundOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inboundOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", inboundOrder.User_ID);
            ViewData["Warehouse_ID"] = new SelectList(_context.Warehouse, "Warehouse_ID", "Warehouse_ID", inboundOrder.Warehouse_ID);
            return View(inboundOrder);
        }

        // GET: InboundOrder/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inboundOrder = await _context.InboundOrder.FindAsync(id);
            if (inboundOrder == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", inboundOrder.User_ID);
            ViewData["Warehouse_ID"] = new SelectList(_context.Warehouse, "Warehouse_ID", "Warehouse_ID", inboundOrder.Warehouse_ID);
            return View(inboundOrder);
        }

        // POST: InboundOrder/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("InboundOrderId,OrderStatus,UserId,Warehouse_ID,EstimatedArrival,ProductQuantity,CreationDate,Cost,Currency,Boxes,InboundType,TrackingNumber,ReferenceOrderNumber,ArrivalMethod")] InboundOrder inboundOrder)
        {
            if (id != inboundOrder.InboundOrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inboundOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InboundOrderExists(inboundOrder.InboundOrderId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", inboundOrder.User_ID);
            ViewData["Warehouse_ID"] = new SelectList(_context.Warehouse, "Warehouse_ID", "Warehouse_ID", inboundOrder.Warehouse_ID);
            return View(inboundOrder);
        }

        // GET: InboundOrder/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inboundOrder = await _context.InboundOrder
                .Include(i => i.User)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(m => m.InboundOrderId == id);
            if (inboundOrder == null)
            {
                return NotFound();
            }

            return View(inboundOrder);
        }

        // POST: InboundOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var inboundOrder = await _context.InboundOrder.FindAsync(id);
            if (inboundOrder != null)
            {
                _context.InboundOrder.Remove(inboundOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InboundOrderExists(string id)
        {
            return _context.InboundOrder.Any(e => e.InboundOrderId == id);
        }
    }
}
