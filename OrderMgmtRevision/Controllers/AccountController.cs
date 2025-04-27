using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using OrderMgmtRevision.Models;
using OrderMgmtRevision.Services;
using System.Security.Claims;
using NuGet.Common;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace OrderMgmtRevision.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogService _logService;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<User> userManager,  SignInManager<User> signInManager, ILogService logService, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logService = logService;
            _emailService = emailService;
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

                if (await _userManager.GetTwoFactorEnabledAsync(user))
                {
                    var twoFactorToken = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

                    return RedirectToAction("Verify2FA", new { userId = user.Id });
                }

                return RedirectToAction("Index", "Dashboard");
            }

            if (result.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(Verify2FA), new { userId = user.Id });
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Your account is locked out.");
                return View(model);
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

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "The email address is already taken.");
                return View(model);
            }

            var user = new User { 
                UserName = model.UserName, 
                Email = model.Email, FullName = model.FullName, 
                DateCreated = DateTime.UtcNow, 
                LastPasswordChange = DateTime.UtcNow, 
                IsActive = false, 
                AccountBalance = 0, 
                EmailConfirmed = false,
                IsAdmin = false
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                // Log user creation
                await _logService.LogUserActivityAsync(user.Id, "Account Created (Pending Admin Approval)", GetClientIp());

                return View("RegistrationPending");
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

        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) { return View(model); }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

            //Email message
            var message = $"Please reset your password by clicking <a href='{callbackUrl}'>here</a>.";

            //Send Email
            await _emailService.SendEmailAsync(user.Email, "Password Reset Request", message);

            await _logService.LogUserActivityAsync(user.Id, "Password Reset Requested", GetClientIp());
            return RedirectToAction("ForgotPasswordConfirmation");
        }

        public IActionResult ForgotPasswordConfirmation() => View();

        public IActionResult ResetPassword(string token, string email) =>
            View(new ResetPasswordViewModel { Token = token, Email = email });

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(Request.Form["Email"]))
            {
                model.Email = Request.Form["Email"];
            }

            if (!ModelState.IsValid) { return View(model); }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return RedirectToAction("ResetPasswordConfirmation");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                await _logService.LogUserActivityAsync(user.Id, "Password Reset", GetClientIp());
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        public IActionResult ResetPasswordConfirmation() => View();

        public async Task<IActionResult> Enable2FA()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // Generate a 2FA key if not already set
            var is2FaEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            if (is2FaEnabled)
            {
                return RedirectToAction("TwoFactorEnabled");
            }

            var authenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user);

            if (string.IsNullOrEmpty(authenticatorKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                authenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            // Generate the QR code URL for an Authenticator App (Google Authenticator)
            var qrCodeUri = GenerateQrCodeUri(user.Email, authenticatorKey);

            // Pass this to the view so the user can scan it
            return View(new Enable2FAViewModel { QrCodeUri = qrCodeUri });
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

            return string.Format(
                AuthenticatorUriFormat,
                Uri.EscapeDataString("YourAppName"),
                Uri.EscapeDataString(email),
                unformattedKey);
        }

        [HttpPost]
        public async Task<IActionResult> Verify2FA(string code)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "Authenticator", code);

            if (isValid)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, true);
                return RedirectToAction("TwoFactorEnabled");  // Redirect to a confirmation page
            }

            ModelState.AddModelError("", "Invalid code.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmailCode(string emailCode)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var result = await _signInManager.TwoFactorSignInAsync("Email", emailCode, false, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError("", "Invalid code.");
            return View();
        }

        public IActionResult TwoFactorEnabled()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
