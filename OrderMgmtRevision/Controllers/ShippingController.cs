using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OrderMgmtRevision.Models;
using Microsoft.AspNetCore.Authorization;
using OrderMgmtRevision.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Azure.Core;


namespace OrderMgmtRevision.Controllers
{
    [Authorize]
   
    public class ShippingController : Controller
    {
        private readonly FedExService _fedExService;
        private readonly ShippoService _shippoService;
        private readonly IWebHostEnvironment _env;
        private static List<FedExShipment> _shipments = new List<FedExShipment>();
        private readonly IStringLocalizer<ShippingController> _localizer;
        private readonly UserManager<User> _userManager;

        // Inject IStringLocalizer for localization
        public ShippingController(FedExService fedExService, IWebHostEnvironment env, IStringLocalizer<ShippingController> localizer, ShippoService shippoService, UserManager<User> userManager)
        {
            _fedExService = fedExService;
            _env = env;
            _localizer = localizer; // Store the localizer
            _shippoService = shippoService;
            _userManager = userManager;
         }

        public IActionResult Index()
        {
            return View(_shipments);
        }

        private async Task<bool> IsUserAdmin()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return false;
            }

            var roles = await _userManager.GetRolesAsync(user);
            return roles.Contains("Admin");
        }

        private string GetClientIp()
        {
            return HttpContext.Connection.RemoteIpAddress?.ToString();
        }

        [HttpPost]
        public async Task<IActionResult> CreateShipment([FromBody] FedExShipment shipment)
        {
            try
            {
                if (shipment == null || !ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    Console.WriteLine($"Validation errors: {string.Join(", ", errors)}");

                    // Use localized string for error message
                    return Json(new { success = false, message = _localizer["Invalid shipment data."] });
                }

                var token = await _fedExService.GetAccessTokenAsync();
                var response = await _fedExService.CreateShipmentAsync(token, shipment);
                dynamic result = JsonConvert.DeserializeObject(response);

                string trackingNumber = result.output.transactionShipments[0].masterTrackingNumber;
                string encodedLabel = result.output.transactionShipments[0].pieceResponses[0].packageDocuments[0].encodedLabel;

                // Decode Label
                byte[] labelBytes = Convert.FromBase64String(encodedLabel);

                // Create path for file download
                string labelPath = Path.Combine(_env.WebRootPath, "labels", $"label_{trackingNumber}.pdf");
                Directory.CreateDirectory(Path.GetDirectoryName(labelPath));
                await System.IO.File.WriteAllBytesAsync(labelPath, labelBytes);

                ViewBag.TrackingNumber = trackingNumber;
                ViewBag.LabelUrl = $"/labels/label_{trackingNumber}.pdf";

                shipment.TrackingNumber = trackingNumber; // Update the shipment with the tracking number
                _shipments.Add(shipment);

                return File(labelBytes, "application/pdf", $"label_{trackingNumber}.pdf");
            }
            catch (Exception ex)
            {
                // Use localized string for error message
                return Json(new { success = false, message = _localizer["Error while creating shipment."] + " " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelShipment(string trackingNumber)
        {
            try
            {
                var token = await _fedExService.GetAccessTokenAsync();
                var response = await _fedExService.CancelShipmentAsync(token, trackingNumber);
                _shipments.RemoveAll(s => s.TrackingNumber == trackingNumber);

                // Use localized string for success message
                return Json(new { success = true, message = _localizer["Shipment {0} canceled successfully.", trackingNumber] });
            }
            catch (Exception ex)
            {
                // Use localized string for error message
                return Json(new { success = false, message = _localizer["Error while canceling shipment."] + " " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult _CreateShipmentRequest()
        {
            return PartialView("_CreateShipmentRequest", new ShippingRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateShippingRequest(ShippingRequest model)
        {
            //bool isAdmin = await IsUserAdmin();
            //var user = await _userManager.GetUserAsync(User);
            //string userId = user?.Id ?? "Unknown";
            //string userName = user?.UserName ?? "Unknown";
            //string ipAddress = GetClientIp();
            //string logMessage = "";


            if (!ModelState.IsValid)
            {
                return View("Index",  model);
            }

            try
            {
                var rates = await _shippoService.GetShippingRatesAsync(model);
                return View("Rates", rates);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error getting rates: {ex.Message}");
                return View("Index", model);
            }

        }


    }
}
