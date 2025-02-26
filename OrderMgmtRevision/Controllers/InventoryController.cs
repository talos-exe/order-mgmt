using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
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

        public InventoryController(FedExService fedExService, IWebHostEnvironment env)
        {
            _fedExService = fedExService;
            _env = env;
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
                    return Json(new { success = false, message = "Invalid shipment data." });
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
                return Json(new { success = false, message = ex.Message });
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
                return Json(new { success = true, message = $"Shipment {trackingNumber} canceled successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
