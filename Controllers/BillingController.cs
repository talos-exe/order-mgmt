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
    public class BillingController : Controller
    {
        private readonly AppDbContext _context;

        public BillingController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Billing
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Billing.Include(b => b.BillingAccount).Include(b => b.Charge);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Billing/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billing = await _context.Billing
                .Include(b => b.BillingAccount)
                .Include(b => b.Charge)
                .FirstOrDefaultAsync(m => m.BillingAccountId == id);
            if (billing == null)
            {
                return NotFound();
            }

            return View(billing);
        }

        // GET: Billing/Create
        public IActionResult Create()
        {
            ViewData["BillingAccountId"] = new SelectList(_context.BillingAccount, "BillingAccountId", "BillingAccountId");
            ViewData["ChargeId"] = new SelectList(_context.Charge, "ChargeId", "ChargeId");
            return View();
        }

        // POST: Billing/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BillingAccountId,ChargeId,Amount,DateCreated")] Billing billing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(billing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BillingAccountId"] = new SelectList(_context.BillingAccount, "BillingAccountId", "BillingAccountId", billing.BillingAccountId);
            ViewData["ChargeId"] = new SelectList(_context.Charge, "ChargeId", "ChargeId", billing.ChargeId);
            return View(billing);
        }

        // GET: Billing/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billing = await _context.Billing.FindAsync(id);
            if (billing == null)
            {
                return NotFound();
            }
            ViewData["BillingAccountId"] = new SelectList(_context.BillingAccount, "BillingAccountId", "BillingAccountId", billing.BillingAccountId);
            ViewData["ChargeId"] = new SelectList(_context.Charge, "ChargeId", "ChargeId", billing.ChargeId);
            return View(billing);
        }

        // POST: Billing/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BillingAccountId,ChargeId,Amount,DateCreated")] Billing billing)
        {
            if (id != billing.BillingAccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(billing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillingExists(billing.BillingAccountId))
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
            ViewData["BillingAccountId"] = new SelectList(_context.BillingAccount, "BillingAccountId", "BillingAccountId", billing.BillingAccountId);
            ViewData["ChargeId"] = new SelectList(_context.Charge, "ChargeId", "ChargeId", billing.ChargeId);
            return View(billing);
        }

        // GET: Billing/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billing = await _context.Billing
                .Include(b => b.BillingAccount)
                .Include(b => b.Charge)
                .FirstOrDefaultAsync(m => m.BillingAccountId == id);
            if (billing == null)
            {
                return NotFound();
            }

            return View(billing);
        }

        // POST: Billing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var billing = await _context.Billing.FindAsync(id);
            if (billing != null)
            {
                _context.Billing.Remove(billing);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillingExists(string id)
        {
            return _context.Billing.Any(e => e.BillingAccountId == id);
        }
    }
}
