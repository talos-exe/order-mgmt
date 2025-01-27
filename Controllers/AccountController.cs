using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Data;
using Microsoft.EntityFrameworkCore; // If using EF for database operations
using System.Threading.Tasks;

namespace OrderManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor to inject the database context
        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // A simple GET action for the Login page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // A POST action to handle login form submission
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid username or password.";
                return View();
            }

            // Here you would sign the user in. For a simple example:
            // (In a production app, you should hash passwords, use Identity, etc.)

            // After successful login, redirect the user to Home page
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password)
        {
            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.Username == username))
            {
                ViewBag.Error = "Username already taken.";
                return View();
            }

            var newUser = new Models.User
            {
                UserId = Guid.NewGuid().ToString("N").Substring(0, 25),
                Username = username,
                Email = email,
                Password = password, // In production, hash this!
                DateCreated = DateTime.Now
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Account");
        }
    }
}