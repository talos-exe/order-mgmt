using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using OrderMgmtRevision.Models;
using Microsoft.AspNetCore.Authorization;
using OrderMgmtRevision.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrderMgmtRevision.Data;
using X.PagedList.Extensions;


namespace OrderMgmtRevision.Controllers
{
    [Authorize]
    public class ShippingController : Controller
    {
        private readonly ShippoService _shippoService;
        private readonly IWebHostEnvironment _env;
        private readonly IStringLocalizer<ShippingController> _localizer;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        private readonly List<ShipmentViewModel> _shipments;

        // Inject IStringLocalizer for localization
        public ShippingController(IWebHostEnvironment env, IStringLocalizer<ShippingController> localizer, ShippoService shippoService, UserManager<User> userManager, AppDbContext context)
        {
            _env = env;
            _localizer = localizer; // Store the localizer
            _shippoService = shippoService;
            _userManager = userManager;
            _context = context;
         }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, string statusFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.StatusFilter = statusFilter;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.TrackingSortParm = sortOrder == "Tracking" ? "tracking_desc" : "Tracking";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "address_desc" : "Address";
            ViewBag.CitySortParm = sortOrder == "City" ? "city_desc" : "City";
            ViewBag.StateSortParm = sortOrder == "State" ? "state_desc" : "State";
            ViewBag.CountrySortParm = sortOrder == "Country" ? "country_desc" : "Country";
            ViewBag.PostalCodeSortParm = sortOrder == "Postal" ? "postal_desc" : "Postal";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            // Fetch the shipments, including related entities
            var shipmentQuery = _context.Shipments
                .Include(s => s.Product) // Include related product information
                .Include(s => s.SourceWarehouse) // Include source warehouse details
                .Include(s => s.ShippingRequest) // Include the shippingrequest
                .Include(s => s.Label) // Include shipping label details
                .Include(s => s.Tracking) // Include tracking details
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                shipmentQuery = shipmentQuery.Where(s =>
                s.ShipmentName.Contains(searchString) ||
                s.TrackingNumber.Contains(searchString) ||
                s.ShippingRequest.ToStreet.Contains(searchString) ||
                s.ShippingRequest.ToCity.Contains(searchString) ||
                s.ShippingRequest.ToState.Contains(searchString) ||
                s.ShippingRequest.ToCountryCode.Contains(searchString) ||
                s.ShippingRequest.ToZip.Contains(searchString));
            }

            if (statusFilter == "cancelled")
            {
                shipmentQuery = shipmentQuery.Where(s => s.Status == "CANCELLED");
            }
            else if (statusFilter == "notcancelled")
            {
                shipmentQuery = shipmentQuery.Where(s => s.Status != "CANCELLED");
            }

            // Sorting
            switch (sortOrder)
            {
                case "name_desc":
                    shipmentQuery = shipmentQuery.OrderByDescending(s => s.ShipmentName);
                    break;
                case "Tracking":
                    shipmentQuery = shipmentQuery.OrderBy(s => s.TrackingNumber);
                    break;
                case "tracking_desc":
                    shipmentQuery = shipmentQuery.OrderByDescending(s => s.TrackingNumber);
                    break;
                case "Address":
                    shipmentQuery = shipmentQuery.OrderBy(s => s.ShippingRequest.ToStreet);
                    break;
                case "address_desc":
                    shipmentQuery = shipmentQuery.OrderByDescending(s => s.ShippingRequest.ToStreet);
                    break;
                case "City":
                    shipmentQuery = shipmentQuery.OrderBy(s => s.ShippingRequest.ToCity);
                    break;
                case "city_desc":
                    shipmentQuery = shipmentQuery.OrderByDescending(s => s.ShippingRequest.ToCity);
                    break;
                case "State":
                    shipmentQuery = shipmentQuery.OrderBy(s => s.ShippingRequest.ToState);
                    break;
                case "state_desc":
                    shipmentQuery = shipmentQuery.OrderByDescending(s => s.ShippingRequest.ToState);
                    break;
                case "Country":
                    shipmentQuery = shipmentQuery.OrderBy(s => s.ShippingRequest.ToCountryCode);
                    break;
                case "country_desc":
                    shipmentQuery = shipmentQuery.OrderByDescending(s => s.ShippingRequest.ToCountryCode);
                    break;
                case "Postal":
                    shipmentQuery = shipmentQuery.OrderBy(s => s.ShippingRequest.ToZip);
                    break;
                case "postal_desc":
                    shipmentQuery = shipmentQuery.OrderByDescending(s => s.ShippingRequest.ToZip);
                    break;
                default:
                    shipmentQuery = shipmentQuery.OrderBy(s => s.ShipmentName);
                    break;
            }

            var shipments = await shipmentQuery.ToListAsync();

            // Map the shipments to the view model
            var shipmentViewModels = shipments.Select(s => new ShipmentViewModel
            {
                ShipmentID = s.ShipmentID.GetValueOrDefault(),
                TrackingNumber = s.TrackingNumber,
                RecipientName = s.ShipmentName,
                Address = s.ShippingRequest?.ToStreet,
                City = s.ShippingRequest?.ToCity,
                CountryCode = s.ShippingRequest?.ToCountryCode,
                State = s.ShippingRequest?.ToState,
                PhoneNumber = s.ShippingRequest?.ToPhone,
                PostalCode = s.ShippingRequest?.ToZip,
                Status = s.Status
            }).ToList();

            int pageSize = 15;
            int pageNumber = (page ?? 1);

            return View(shipmentViewModels.ToPagedList(pageNumber, pageSize));
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

        [HttpGet]
        public async Task<IActionResult> _CreateShipmentRequest()
        {
            var model = new ShippingRequestViewModel
            {
                ProductList = await _context.Products.ToListAsync(),
                Warehouses = await _context.Warehouses.ToListAsync()
            };

            return PartialView("_CreateShipmentRequest", model);
        }

        [HttpPost]
        public async Task<IActionResult> GetRates(ShippingRequestViewModel request)
        {
            var rates = await _shippoService.GetShippingRatesAsync(request); // however you're getting these

            var result = rates.Select(r => new {
                RateObjectId = r.RateObjectId,
                Provider = r.Provider,
                Service = r.Service,
                Amount = r.Amount,
                Currency = r.Currency,
                EstimatedDays = r.EstimatedDays
            }).ToList(); // <--- this is important!
            System.Diagnostics.Debug.WriteLine("Shippo rate results when clicking shipment: " + JsonConvert.SerializeObject(result));

            return Json(result);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateShippingRequest(ShippingRequestViewModel model, string selectedRateJson)
        {
            bool isAdmin = await IsUserAdmin();
            var user = await _userManager.GetUserAsync(User);
            string userId = user?.Id ?? "Unknown";
            string userName = user?.UserName ?? "Unknown";
            string ipAddress = GetClientIp();
            string logMessage = "";

            System.Diagnostics.Debug.WriteLine("CreateShippingRequest called with Rate JSON: " + selectedRateJson);

            ModelState.Remove("Warehouses");
            ModelState.Remove("ProductList");

            if (model.SourceWarehouseId > 0)
            {
                var warehouse = await _context.Warehouses.FindAsync(model.SourceWarehouseId);
                if (warehouse != null)
                {
                    // Populate the from address fields from the warehouse
                    model.FromName = warehouse.WarehouseName;
                    model.FromStreet = warehouse.Address;
                    model.FromCity = warehouse.City;
                    model.FromState = warehouse.State;
                    model.FromZip = warehouse.Zip;
                    model.FromEmail = warehouse.WarehouseEmail;
                    model.FromPhone = warehouse.PhoneNumber;
                }
            }

            if (!ModelState.IsValid)
            {
                // Repopulate dropdown lists
                model.ProductList = await _context.Products.ToListAsync();
                model.Warehouses = await _context.Warehouses.ToListAsync();
                return View("Index", model);
            }

            var source = await _context.Warehouses.FindAsync(model.SourceWarehouseId);

            var request = new ShippingRequest
            {
                FromName = source.WarehouseName,
                FromStreet = source.Address,
                FromCity = source.City,
                FromState = source.State,
                FromZip = source.Zip,
                FromPhone = source.PhoneNumber,
                FromEmail = source.WarehouseEmail,
                ToName = model.ToName,
                ToStreet = model.ToStreet,
                ToCity = model.ToCity,
                ToState = model.ToState,
                ToZip = model.ToZip,
                ToCountryCode = model.ToCountryCode,
                ToPhone = model.ToPhone,
                Weight = model.Weight,
                Length = model.Length,
                Width = model.Width,
                Height = model.Height
            };

            ShippingRate selectedRate = null;
            if (!string.IsNullOrEmpty(selectedRateJson))
            {
                try
                {
                    selectedRate = JsonConvert.DeserializeObject<ShippingRate>(selectedRateJson);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error deserializing rate: " + ex.Message);
                }
            }

            System.Diagnostics.Debug.WriteLine("Selected rate chosen: " + JsonConvert.SerializeObject(selectedRate));

            if (selectedRate == null)
            {
                TempData["Error"] = "Selected rate not found.";
                return RedirectToAction("Index", "Shipping");
            }

            var label = await _shippoService.CreateLabelAsync(selectedRate.RateObjectId);
            var trackingNumber = string.IsNullOrWhiteSpace(label.TrackingNumber) ? "NOTGIVEN" : label.TrackingNumber;

            var shipment = new Shipment
            {
                ShipmentName = model.ToName,
                ProductID = model.ProductID,
                SourceWarehouseID = model.SourceWarehouseId,
                Status = "CREATED",
                Cost = selectedRate.Amount,
                TrackingNumber = trackingNumber,
                SelectedRateId = selectedRate.RateObjectId,
                Rate = selectedRate,
                Label = new ShippingLabel
                {
                    LabelUrl = label.LabelUrl,
                    LabelObjectId = label.LabelObjectId,
                    TrackingNumber = label.TrackingNumber,
                    TrackingUrl = label.TrackingUrl
                },
                Tracking = new ShipmentTracking
                {
                    Location = model.FromStreet,
                    StatusDate = "",
                    Status = "CREATED"
                },
                ShippingRequest = request,
                GeneratedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userName
            };

            try
            {
                _context.Shipments.Add(shipment);
                var result = await _context.SaveChangesAsync();
                // Check result - should be > 0 if successful
                if (result > 0)
                {
                    TempData["SuccessMessage"] = "Shipment created successfully with tracking number: " + shipment.TrackingNumber;
                }
                else
                {
                    TempData["Error"] = "No changes were saved to the database.";
                }
                return RedirectToAction("Index", "Shipping");
            }
            catch (Exception ex)
            {
                // Log the exception
                TempData["Error"] = "Error creating shipment: " + ex.Message;
                if (ex.InnerException != null)
                {
                    TempData["Errors"] = new List<string> { ex.InnerException.Message };
                }
                return RedirectToAction("Index", "Shipping");
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetShipmentDetails(int shipmentId)
        {
            var shipment = await _context.Shipments.FindAsync(shipmentId);
            if (shipment == null)
            {
                return NotFound();
            }

            var shipmentViewModel = await _context.Shipments
                .Where(s => s.ShipmentID == shipmentId)
                .AsNoTracking()
                .Select(s => new Shipment
                {
                    ShipmentID = s.ShipmentID,
                    ShipmentName = s.ShipmentName,
                    Rate = s.Rate,
                    TrackingNumber = s.TrackingNumber,
                    Cost = s.Cost,
                    SourceWarehouse = s.SourceWarehouse,
                    Product = s.Product,
                    ShippingRequest = s.ShippingRequest,
                    Label = s.Label,
                    GeneratedAt = s.GeneratedAt,
                    UpdatedAt = s.UpdatedAt
                })
                .FirstOrDefaultAsync();

            return PartialView("_ShipmentDetails", shipmentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelShipment (int id)
        {
            var shipment = await _context.Shipments
                .Include(s => s.Label)
                .Include(s => s.Tracking)
                .Include(s => s.ShippingRequest)
                .FirstOrDefaultAsync(s => s.ShipmentID == id);

            if (shipment == null)
            {
                TempData["Error"] = "Shipment not found.";
                return RedirectToAction("Index");
            }

            System.Diagnostics.Debug.WriteLine($"Attempting to cancel label with ID: {shipment.Label.LabelObjectId}");
            
            try
            {
                bool cancelled = await _shippoService.CancelShipmentAsync(shipment.Label.LabelObjectId);
                System.Diagnostics.Debug.WriteLine("Cancel shipment result: " + cancelled);
                if (!cancelled)
                {
                    TempData["Error"] = "Unable to cancel shipment. It may have already been picked up or processed.";
                    return RedirectToAction("Index");
                }

                shipment.Status = "CANCELLED";
                shipment.Tracking.Status = "CANCELLED";
                shipment.Tracking.StatusDate = DateTime.UtcNow.ToString("yyy-MM-ddTHH:mm:ssZ");

                _context.Shipments.Update(shipment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Shipment cancelled successfully.";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception during shipment cancellation: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine("$Inner exception: {ex.InnerException.Message}");
                }
                TempData["Error"] = "Error cancelling shipment: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

    }
}
