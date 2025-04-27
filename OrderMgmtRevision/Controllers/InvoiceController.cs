using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using OrderMgmtRevision.Models;
using OrderMgmtRevision.Services;

namespace OrderMgmtRevision.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        private readonly StripeService _stripeService;
        private readonly UserManager<User> _userManager;
        private readonly ILogService _logService;

        public InvoiceController(StripeService stripeService, UserManager<User> userManager, ILogService logService)
        {
            _stripeService = stripeService;
            _userManager = userManager;
            _logService = logService;
        }

        public IActionResult Index()
        {
            return View();
        }

        private string GetClientIp()
        {
            return HttpContext.Connection.RemoteIpAddress?.ToString();
        }

        [HttpPost]
        [Authorize] // Ensure the user is logged in
        public async Task<IActionResult> DirectCheckout(long amount)
        {
            string ipAddress = GetClientIp();

            try
            {
                // Get the current user's email
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                string userEmail = user.Email;

                string baseSuccessUrl = Url.Action("PaymentSuccess", "Invoice", null, Request.Scheme);
                string baseCancelUrl = Url.Action("PaymentCancel", "Invoice", null, Request.Scheme);

                string successUrl = $"{baseSuccessUrl}?sessionId={{CHECKOUT_SESSION_ID}}";
                string cancelUrl = $"{baseCancelUrl}?sessionId={{CHECKOUT_SESSION_ID}}";

                HttpContext.Session.SetString("AccessGranted", "true");

                string checkoutUrl = await _stripeService.CreateDirectCheckoutSessionAsync(userEmail, amount, successUrl, cancelUrl);
                await _logService.LogUserActivityAsync(user.Id, $"Started checkout session.", ipAddress);
                return Redirect(checkoutUrl);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error creating checkout session: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> PaymentSuccess(string sessionId)
        {
            string ipAddress = GetClientIp();
            // Check if the session is valid
            var access = HttpContext.Session.GetString("AccessGranted");
            if (access != "true")
            {
                return RedirectToAction("Index", "Dashboard"); // Use only the controller name without the prefix
            }

            // Optional: Remove session after accessing the page
            HttpContext.Session.Remove("AccessGranted");

            var user = await _userManager.GetUserAsync(User);
            if (user != null && user.AccountBalance > 0)
            {
                // Reset the account balance to zero after successful payment
                var oldAccountBalance = user.AccountBalance;
                user.AccountBalance = 0;

                // Save the changes to the database
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["Toast"] = "success|Payment successful. Your account balance has been updated.";
                    await _logService.LogUserActivityAsync(user.Id, $"Successfully paid invoice amount ${(oldAccountBalance/100)?.ToString("F2")}.", ipAddress);
                }
                else
                {
                    TempData["Toast"] = "error|Payment was processed but we couldn't update your account balance. Please contact support.";
                    await _logService.LogUserActivityAsync(user.Id, "[CRITICAL] Was unable to pay invoice, payment was processed.", ipAddress);
                }
            }

            ViewBag.SessionId = sessionId;
            return View("PaymentSuccess");
        }

        public async Task<IActionResult> PaymentCancel(string sessionId)
        {
            string ipAddress = GetClientIp();
            var user = await _userManager.GetUserAsync(User);
            // Check if the session is valid
            var access = HttpContext.Session.GetString("AccessGranted");
            if (access != "true")
            {
                return RedirectToAction("Index", "Dashboard"); // Correct relative redirect
            }

            // Optional: Remove session after accessing the page
            HttpContext.Session.Remove("AccessGranted");

            TempData["Toast"] = "error|Payment was cancelled. Your account balance remains unchanged.";
            await _logService.LogUserActivityAsync(user.Id, "Payment for invoice cancelled.", ipAddress);

            return View("PaymentCancel");
        }
    }
}