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

        public InvoiceController(StripeService stripeService, UserManager<User> userManager)
        {
            _stripeService = stripeService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var model = new StripePaymentModel
            {
                Amount = 1000 // example, need to pass in value after invoices implemented (cents)
            };
            return View(model);
        }

        [HttpGet]
        [Authorize] // Ensure the user is logged in
        public async Task<IActionResult> DirectCheckout(long amount)
        {
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
                return Redirect(checkoutUrl);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error creating checkout session: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult PaymentSuccess(string sessionId)
        {
            // Check if the session is valid
            var access = HttpContext.Session.GetString("AccessGranted");
            if (access != "true")
            {
                return RedirectToAction("Index", "Dashboard"); // Use only the controller name without the prefix
            }

            // Optional: Remove session after accessing the page
            HttpContext.Session.Remove("AccessGranted");

            ViewBag.SessionId = sessionId;
            return View("PaymentSuccess");
        }

        public IActionResult PaymentCancel(string sessionId)
        {
            // Check if the session is valid
            var access = HttpContext.Session.GetString("AccessGranted");
            if (access != "true")
            {
                return RedirectToAction("Index", "Dashboard"); // Correct relative redirect
            }

            // Optional: Remove session after accessing the page
            HttpContext.Session.Remove("AccessGranted");

            return View("PaymentCancel");
        }
    }
}