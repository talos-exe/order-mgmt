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
    public class ChargeController : Controller
    {
        private readonly AppDbContext _context;

        public ChargeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Charge
        public async Task<IActionResult> Index()
        {
            return View(await _context.Charge.ToListAsync());
        }

        // GET: Charge/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var charge = await _context.Charge
                .FirstOrDefaultAsync(m => m.ChargeId == id);
            if (charge == null)
            {
                return NotFound();
            }

            return View(charge);
        }

        // GET: Charge/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Charge/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChargeId,Amount,ChargeType,Description")] Charge charge)
        {
            if (ModelState.IsValid)
            {
                _context.Add(charge);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(charge);
        }

        // GET: Charge/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var charge = await _context.Charge.FindAsync(id);
            if (charge == null)
            {
                return NotFound();
            }
            return View(charge);
        }

        // POST: Charge/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ChargeId,Amount,ChargeType,Description")] Charge charge)
        {
            if (id != charge.ChargeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(charge);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChargeExists(charge.ChargeId))
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
            return View(charge);
        }

        // GET: Charge/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var charge = await _context.Charge
                .FirstOrDefaultAsync(m => m.ChargeId == id);
            if (charge == null)
            {
                return NotFound();
            }

            return View(charge);
        }

        // POST: Charge/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var charge = await _context.Charge.FindAsync(id);
            if (charge != null)
            {
                _context.Charge.Remove(charge);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChargeExists(string id)
        {
            return _context.Charge.Any(e => e.ChargeId == id);
        }
    }
}
