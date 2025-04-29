using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using OrderMgmtRevision.Data;
using OrderMgmtRevision.Models;
using OrderMgmtRevision.Services;
using X.PagedList.Extensions;

namespace OrderMgmtRevision.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        private readonly StripeService _stripeService;
        private readonly UserManager<User> _userManager;
        private readonly ILogService _logService;
        private readonly AppDbContext _dbContext;

        public InvoiceController(StripeService stripeService, UserManager<User> userManager, ILogService logService, AppDbContext dbContext)
        {
            _stripeService = stripeService;
            _userManager = userManager;
            _logService = logService;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        private string GetClientIp()
        {
            return HttpContext.Connection.RemoteIpAddress?.ToString();
        }

        public async Task<IActionResult> InvoiceHistory(int? page)
        {
            var userId = _userManager.GetUserId(User);
            var accountBalance = await _dbContext.UserInvoices
                    .Where(i => i.UserId == userId && !i.IsPaid)
                    .SumAsync(i => i.InvoiceAmount);

            var invoicesQuery = _dbContext.UserInvoices
                .Where(i => i.UserId == userId)
                .OrderByDescending(i => i.DateCreated);

            int pageSize = 15;
            int pageNumber = (page ?? 1);

            ViewBag.UserId = userId;
            ViewBag.AccountBalance = accountBalance;

            return View(invoicesQuery.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        [Authorize] // Ensure the user is logged in
        public async Task<IActionResult> DirectCheckout(Guid invoiceId)
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

                var invoice = await _dbContext.UserInvoices.FindAsync(invoiceId);
                if (invoice == null || invoice.IsPaid) { TempData["Toast"] = "error|Invoice not found."; return RedirectToAction("InvoiceHistory"); }
                if (invoice.IsPaid) {
                    TempData["Toast"] = "info|This invoice has already been paid.";
                    return RedirectToAction("InvoiceHistory");
                }

                var amount = invoice.InvoiceAmount;

                string userEmail = user.Email;

                string baseSuccessUrl = Url.Action("PaymentSuccess", "Invoice", null, Request.Scheme);
                string baseCancelUrl = Url.Action("PaymentCancel", "Invoice", null, Request.Scheme);

                string successUrl = $"{baseSuccessUrl}?sessionId={{CHECKOUT_SESSION_ID}}";
                string cancelUrl = $"{baseCancelUrl}?sessionId={{CHECKOUT_SESSION_ID}}";

                HttpContext.Session.SetString("PendingInvoiceId", invoiceId.ToString());
                HttpContext.Session.SetString("PaymentType", "single");
                HttpContext.Session.SetString("AccessGranted", "true");

                string checkoutUrl = await _stripeService.CreateDirectCheckoutSessionAsync(userEmail, (long) amount, successUrl, cancelUrl);
                await _logService.LogUserActivityAsync(user.Id, $"Started checkout session.", ipAddress);
                return Redirect(checkoutUrl);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error creating checkout session: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize] // Ensure the user is logged in
        public async Task<IActionResult> DirectCheckoutAccountBalance(string userId)
        {
            string ipAddress = GetClientIp();

            try
            {
                // Get the current user's email
                var totalDue = _dbContext.UserInvoices
                    .Where(i => i.UserId == userId && !i.IsPaid)
                    .Sum(i => i.InvoiceAmount);

                if (totalDue <= 0)
                {
                    TempData["Toast"] = "info|No balance due.";
                    return RedirectToAction("InvoiceHistory");
                }

                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }


                string userEmail = user.Email;

                string baseSuccessUrl = Url.Action("PaymentSuccess", "Invoice", null, Request.Scheme);
                string baseCancelUrl = Url.Action("PaymentCancel", "Invoice", null, Request.Scheme);

                string successUrl = $"{baseSuccessUrl}?sessionId={{CHECKOUT_SESSION_ID}}";
                string cancelUrl = $"{baseCancelUrl}?sessionId={{CHECKOUT_SESSION_ID}}";

                HttpContext.Session.SetString("PaymentType", "full");
                HttpContext.Session.SetString("UserId", userId);
                HttpContext.Session.SetString("AccessGranted", "true");

                string checkoutUrl = await _stripeService.CreateDirectCheckoutSessionAsync(userEmail, (long)totalDue, successUrl, cancelUrl);
                await _logService.LogUserActivityAsync(user.Id, $"Started checkout session for account balance ${(totalDue / 100).ToString("F2")}.", ipAddress);
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

            var paymentType = HttpContext.Session.GetString("PaymentType");
            var user = await _userManager.GetUserAsync(User);
            if (user == null) { return RedirectToAction("Login", "Account"); }
            
            if (paymentType == "single")
            {
                var invoiceIdStr = HttpContext.Session.GetString("PendingInvoiceId");

                if (!string.IsNullOrEmpty(invoiceIdStr) && Guid.TryParse(invoiceIdStr, out Guid invoiceId)) {
                    var invoice = await _dbContext.UserInvoices.FindAsync(invoiceId);
                    if (invoice != null && !invoice.IsPaid)
                    {
                        invoice.IsPaid = true;
                        invoice.DatePaid = DateTime.Now;
                        invoice.PaymentReference = sessionId;
                        user.AccountBalance -= invoice.InvoiceAmount;

                        await _dbContext.SaveChangesAsync();
                        await _logService.LogUserActivityAsync(user.Id, $"Invoice {invoiceId} marked as paid. Amount: ${((double)invoice.InvoiceAmount / 100.0).ToString("F2")}", ipAddress);
                        TempData["Toast"] = "success|Payment successful! Your invoice has been marked as paid.";
                    }
                }
            }
            else if (paymentType == "full")
            {
                 // Mark all unpaid invoices for this user as paid
                var unpaidInvoices = await _dbContext.UserInvoices
                    .Where(i => i.UserId == user.Id && !i.IsPaid)
                    .ToListAsync();
                
                decimal totalPaid = 0;
                
                foreach (var invoice in unpaidInvoices)
                {
                    invoice.IsPaid = true;
                    invoice.DatePaid = DateTime.UtcNow;
                    invoice.PaymentReference = sessionId;
                    totalPaid += invoice.InvoiceAmount;
                }
                user.AccountBalance = 0;

                await _dbContext.SaveChangesAsync();
                await _logService.LogUserActivityAsync(user.Id, $"All invoices marked as paid. Total: ${((double)totalPaid / 100.0).ToString("F2")}", ipAddress);
                TempData["Toast"] = "success|Payment successful! Your account balance has been cleared.";
            }

            HttpContext.Session.Remove("AccessGranted");
            HttpContext.Session.Remove("PaymentType");
            HttpContext.Session.Remove("PendingInvoiceId");
            HttpContext.Session.Remove("UserId");

            ViewBag.SessionId = sessionId;
            return View("PaymentSuccess");
        }

        public async Task<IActionResult> PaymentCancel(string sessionId)
        {
            string ipAddress = GetClientIp();
            // Check if the session is valid
            var access = HttpContext.Session.GetString("AccessGranted");
            if (access != "true")
            {
                return RedirectToAction("Index", "Dashboard"); // Correct relative redirect
            }

            var user = await _userManager.GetUserAsync(User);

            // Optional: Remove session after accessing the page
            HttpContext.Session.Remove("AccessGranted");
            HttpContext.Session.Remove("PaymentType");
            HttpContext.Session.Remove("PendingInvoiceId");
            HttpContext.Session.Remove("UserId");

            TempData["Toast"] = "error|Payment was cancelled. Your account balance remains unchanged.";
            await _logService.LogUserActivityAsync(user.Id, "Payment for invoice cancelled.", ipAddress);

            return View("PaymentCancel");
        }
    }
}