using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderMgmtRevision.Data;
using OrderMgmtRevision.Models;
using System.Security.Claims;

namespace OrderMgmtRevision.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public SettingsController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async  Task<IActionResult> Index()
        {
            // Get logged-in user ID from claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return NotFound();

            // Get the user and their account data
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return NotFound();

            // Count the products and shipments created by this user
            var productsCount = await _context.Products.CountAsync(p => p.CreatedBy == userId);
            var shipmentsCount = await _context.Shipments.CountAsync(s => s.CreatedBy == userId);

            var viewModel = new UserViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                User = user,
                AccountBalance = user.AccountBalance
            };


            return View(viewModel);
        }
    }
}
