using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OrderMgmtRevision.Models;


namespace OrderMgmtRevision.Controllers
{
    public class InventoryController : Controller
    {
        private readonly FedExService _fedExService;
        private readonly IWebHostEnvironment _env;
        private static List<FedExShipment> _shipments = new List<FedExShipment>();
        private readonly IStringLocalizer<InventoryController> _localizer;

        // Inject IStringLocalizer for localization
        public InventoryController(FedExService fedExService, IWebHostEnvironment env, IStringLocalizer<InventoryController> localizer)
        {
            _fedExService = fedExService;
            _env = env;
            _localizer = localizer; // Store the localizer
        }

        public IActionResult Index()
        {
            return View(_shipments);
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
    }
}
