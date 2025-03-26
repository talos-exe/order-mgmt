using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using OrderMgmtRevision.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OrderMgmtRevision.Services;
using System.Security.Claims;

namespace OrderMgmtRevision.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogService _logService;

        public AccountController(UserManager<User> userManager,  SignInManager<User> signInManager, ILogService logService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logService = logService;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                ModelState.AddModelError("", "User account not found.");
                await _logService.LogUserActivityAsync("Anonymous", "Failed Login Attempt", GetClientIp());  // Log failed login attempt
                return View(model);
            }

            if (!user.IsActive)
            {
                ModelState.AddModelError("", "Your account is deactivated. Please contact support.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, true, false);

            if (result.Succeeded)
            {
                user.LastLogin = DateTime.UtcNow;
                user.LastLoginIP = GetClientIp();
                await _userManager.UpdateAsync(user);
                await _logService.LogUserActivityAsync(user.Id, "Logged In", GetClientIp());
                return RedirectToAction("Index", "Dashboard");
            }
            else if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Your account is locked out.");
            }

            ModelState.AddModelError("", "Invalid login attempt.");

            // Log it
            await _logService.LogUserActivityAsync(user?.Id, "Failed Login Attempt", GetClientIp());

            return View(model);
        }

        private string GetClientIp()
        {
            return HttpContext.Connection.RemoteIpAddress?.ToString();
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new User { UserName = model.UserName, Email = model.Email, FullName = model.FullName, DateCreated = DateTime.UtcNow, LastPasswordChange = DateTime.UtcNow, IsActive = true };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                // Log user creation
                await _logService.LogUserActivityAsync(user.Id, "Account Created", GetClientIp());

                if (User.IsInRole("Admin"))
                {
                    // For admin-created users, do not sign in as the new user.
                    return RedirectToAction("Index", "UserManagement");
                }
                await _signInManager.SignInAsync(user, isPersistent: false);

                // Log successful login after registration
                await _logService.LogUserActivityAsync(user.Id, "Logged In", GetClientIp());

                return RedirectToAction("Index", "Dashboard");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _signInManager.SignOutAsync();

            // Log user logout
            await _logService.LogUserActivityAsync(userId, "Logged Out", GetClientIp());

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
