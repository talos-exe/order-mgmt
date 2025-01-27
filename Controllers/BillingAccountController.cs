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
    public class BillingAccountController : Controller
    {
        private readonly AppDbContext _context;

        public BillingAccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: BillingAccount
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.BillingAccount.Include(b => b.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: BillingAccount/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billingAccount = await _context.BillingAccount
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BillingAccountId == id);
            if (billingAccount == null)
            {
                return NotFound();
            }

            return View(billingAccount);
        }

        // GET: BillingAccount/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: BillingAccount/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BillingAccountId,UserId,AccountBalance")] BillingAccount billingAccount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(billingAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", billingAccount.UserId);
            return View(billingAccount);
        }

        // GET: BillingAccount/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billingAccount = await _context.BillingAccount.FindAsync(id);
            if (billingAccount == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", billingAccount.UserId);
            return View(billingAccount);
        }

        // POST: BillingAccount/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BillingAccountId,UserId,AccountBalance")] BillingAccount billingAccount)
        {
            if (id != billingAccount.BillingAccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(billingAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillingAccountExists(billingAccount.BillingAccountId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", billingAccount.UserId);
            return View(billingAccount);
        }

        // GET: BillingAccount/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billingAccount = await _context.BillingAccount
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BillingAccountId == id);
            if (billingAccount == null)
            {
                return NotFound();
            }

            return View(billingAccount);
        }

        // POST: BillingAccount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var billingAccount = await _context.BillingAccount.FindAsync(id);
            if (billingAccount != null)
            {
                _context.BillingAccount.Remove(billingAccount);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillingAccountExists(string id)
        {
            return _context.BillingAccount.Any(e => e.BillingAccountId == id);
        }
    }
}
