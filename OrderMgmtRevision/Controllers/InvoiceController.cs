using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderMgmtRevision.Models;
using OrderMgmtRevision.Services;

namespace OrderMgmtRevision.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly StripeService _stripeService;

        public InvoiceController(StripeService stripeService)
        {
            _stripeService = stripeService;
        }

        public IActionResult Index()
        {
            var model = new StripePaymentModel
            {
                Amount = 1000 // example, need to pass in value after invoices implemented (cents)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SavePaymentMethod([FromBody] SavePaymentMethodRequest request)
        {
            var (success, error, customerId) = await _stripeService.SavePaymentMethodAsync(request.PaymentMethodId, request.Email);
            return Json(new { success, error, customerId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CreateCheckoutRequest request)
        {
            try
            {
                string sessionId;
                if (request.PaymentType == "card")
                {
                    sessionId = await _stripeService.CreateCardCheckoutSessionAsync(request.CustomerId, request.Amount);
                }
                else if (request.PaymentType == "ach")
                {
                    sessionId = await _stripeService.CreateACHCheckoutSessionAsync(request.CustomerId, request.Amount);
                }
                else
                {
                    return Json(new { success = false, error = "Invalid payment type" });
                }
                return Json(new { success = true, sessionId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}