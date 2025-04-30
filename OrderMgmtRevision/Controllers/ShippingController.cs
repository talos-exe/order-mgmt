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
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Common;
using Stripe;
using Stripe.Climate;


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
        private readonly ILogService _logService;


        // Inject IStringLocalizer for localization
        public ShippingController(IWebHostEnvironment env, IStringLocalizer<ShippingController> localizer, ShippoService shippoService, UserManager<User> userManager, AppDbContext context, ILogService logService)
        {
            _env = env;
            _localizer = localizer; // Store the localizer
            _shippoService = shippoService;
            _userManager = userManager;
            _context = context;
            _logService = logService;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, string statusFilter, int? page)
        {
            var user = await _userManager.GetUserAsync(User);
            string userName = user?.UserName ?? "Unknown";
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");


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

            if (!isAdmin)
            {
                shipmentQuery = shipmentQuery.Where(s => s.CreatedBy == userName);
            }

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

            ViewBag.ShippingOutbound = await shipmentQuery.CountAsync();

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


        private async Task<bool> SendUserShippingInvoice(User userAccount, decimal chargeAmount, Shipment shipment)
        {
            if (userAccount != null)
            {
                try
                {
                    var invoice = new UserInvoice
                    {
                        Shipment = shipment,
                        ShipmentId = shipment.ShipmentID,
                        InvoiceAmount = chargeAmount,
                        DateCreated = DateTime.UtcNow,
                        DateDue = DateTime.UtcNow.AddDays(7),
                        IsPaid = false,
                        UserId = userAccount.Id,
                        User = userAccount,
                        Description = $"Invoice Amount {chargeAmount} created due to shipment {shipment.ShipmentID}, charged to user {userAccount.UserName}"
                    };

                    userAccount.AccountBalance += chargeAmount;

                    _context.Add(invoice);
                    int saveResult = _context.SaveChanges();
                    await _logService.LogUserActivityAsync(userAccount.Id, $"Invoice amount ${(invoice.InvoiceAmount/100).ToString("F2")} created from shipment ID {shipment.ShipmentID}", GetClientIp());
                    return saveResult > 0;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("SendUserShippingInvoice Error: " + ex.ToString());
                    return false;
                }
            }
            return false;
        }

        [HttpGet]
        public async Task<IActionResult> _CreateShipmentRequest()
        {
            var model = new ShippingRequestViewModel
            {
                ProductList = await _context.Products.ToListAsync(),
                Warehouses = await _context.Warehouses.ToListAsync(),
                StateList = GetStateSelectList()
            };

            return PartialView("_CreateShipmentRequest", model);
        }

        private List<SelectListItem> GetStateSelectList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Alabama", Value = "AL" },
                new SelectListItem { Text = "Alaska", Value = "AK" },
                new SelectListItem { Text = "Arizona", Value = "AZ" },
                new SelectListItem { Text = "Arkansas", Value = "AR" },
                new SelectListItem { Text = "California", Value = "CA" },
                new SelectListItem { Text = "Colorado", Value = "CO" },
                new SelectListItem { Text = "Connecticut", Value = "CT" },
                new SelectListItem { Text = "Delaware", Value = "DE" },
                new SelectListItem { Text = "Florida", Value = "FL" },
                new SelectListItem { Text = "Georgia", Value = "GA" },
                new SelectListItem { Text = "Hawaii", Value = "HI" },
                new SelectListItem { Text = "Idaho", Value = "ID" },
                new SelectListItem { Text = "Illinois", Value = "IL" },
                new SelectListItem { Text = "Indiana", Value = "IN" },
                new SelectListItem { Text = "Iowa", Value = "IA" },
                new SelectListItem { Text = "Kansas", Value = "KS" },
                new SelectListItem { Text = "Kentucky", Value = "KY" },
                new SelectListItem { Text = "Louisiana", Value = "LA" },
                new SelectListItem { Text = "Maine", Value = "ME" },
                new SelectListItem { Text = "Maryland", Value = "MD" },
                new SelectListItem { Text = "Massachusetts", Value = "MA" },
                new SelectListItem { Text = "Michigan", Value = "MI" },
                new SelectListItem { Text = "Minnesota", Value = "MN" },
                new SelectListItem { Text = "Mississippi", Value = "MS" },
                new SelectListItem { Text = "Missouri", Value = "MO" },
                new SelectListItem { Text = "Montana", Value = "MT" },
                new SelectListItem { Text = "Nebraska", Value = "NE" },
                new SelectListItem { Text = "Nevada", Value = "NV" },
                new SelectListItem { Text = "New Hampshire", Value = "NH" },
                new SelectListItem { Text = "New Jersey", Value = "NJ" },
                new SelectListItem { Text = "New Mexico", Value = "NM" },
                new SelectListItem { Text = "New York", Value = "NY" },
                new SelectListItem { Text = "North Carolina", Value = "NC" },
                new SelectListItem { Text = "North Dakota", Value = "ND" },
                new SelectListItem { Text = "Ohio", Value = "OH" },
                new SelectListItem { Text = "Oklahoma", Value = "OK" },
                new SelectListItem { Text = "Oregon", Value = "OR" },
                new SelectListItem { Text = "Pennsylvania", Value = "PA" },
                new SelectListItem { Text = "Rhode Island", Value = "RI" },
                new SelectListItem { Text = "South Carolina", Value = "SC" },
                new SelectListItem { Text = "South Dakota", Value = "SD" },
                new SelectListItem { Text = "Tennessee", Value = "TN" },
                new SelectListItem { Text = "Texas", Value = "TX" },
                new SelectListItem { Text = "Utah", Value = "UT" },
                new SelectListItem { Text = "Vermont", Value = "VT" },
                new SelectListItem { Text = "Virginia", Value = "VA" },
                new SelectListItem { Text = "Washington", Value = "WA" },
                new SelectListItem { Text = "West Virginia", Value = "WV" },
                new SelectListItem { Text = "Wisconsin", Value = "WI" },
                new SelectListItem { Text = "Wyoming", Value = "WY" }
            };
        }

        [HttpPost]
        public async Task<IActionResult> GetRates(ShippingRequestViewModel request)
        {
            ModelState.Remove("Warehouses");
            ModelState.Remove("ProductList");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

                return BadRequest(new { errors });
            }

            var product = await _context.Products.FindAsync(request.ProductID);
            if (product == null)
            {
                return BadRequest("Invalid product selected");
            }

            // Ensure we're using server-side values, not client-provided ones
            request.Weight = (decimal)product.Weight;
            request.Length = (decimal)product.Length;
            request.Width = (decimal)product.Width;
            request.Height = (decimal)product.Height;

            var rates = await _shippoService.GetShippingRatesAsync(request);

            // Store rates in session to verify selection later
            var sanitizedRates = rates.Select(r => new {
                RateObjectId = r.RateObjectId,
                Provider = r.Provider,
                Service = r.Service,
                Amount = r.Amount,
                Currency = r.Currency,
                EstimatedDays = r.EstimatedDays
            }).ToList();

            // Store valid rate IDs and amounts in TempData or Session for validation during submission
            HttpContext.Session.SetString("ValidRates", JsonConvert.SerializeObject(sanitizedRates));

            return Json(sanitizedRates);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateShippingRequest(ShippingRequestViewModel model, string selectedRateId)
        {
            bool isAdmin = await IsUserAdmin();
            var user = await _userManager.GetUserAsync(User);
            string userId = user?.Id ?? "Unknown";
            string userName = user?.UserName ?? "Unknown";
            string ipAddress = GetClientIp();

            Models.Product? originalProductObj = await _context.Products.FindAsync(model.ProductID);
            if (originalProductObj == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid product selected.");
                return View(model);
            }

            model.Weight = (decimal)originalProductObj.Weight;
            model.Height = (decimal)originalProductObj.Height;
            model.Length = (decimal)originalProductObj.Length;
            model.Width = (decimal)originalProductObj.Width;

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
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["Errors"] = errors;
                return RedirectToAction("Index", "Shipping");
            }

            var source = await _context.Warehouses.FindAsync(model.SourceWarehouseId);
            if (source == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid warehouse selected");
                return View(model);
            }


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

            string validRatesJson = HttpContext.Session.GetString("ValidRates");
            if (string.IsNullOrEmpty(validRatesJson))
            {
                TempData["Error"] = "Your session has expired. Please try again.";
                return RedirectToAction("Index");
            }

            var validRates = JsonConvert.DeserializeObject<List<Models.ShippingRate>>(validRatesJson);
            var selectedRate = validRates?.FirstOrDefault(r => r.RateObjectId == selectedRateId);

            if (selectedRate == null)
            {
                await _logService.LogUserActivityAsync(userId, $"User {userName} attempted to use an invalid rate ID: {selectedRateId}", ipAddress);
                TempData["Error"] = "Selected shipping rate is invalid. Please try again.";
                return RedirectToAction("Index");
            }

            // Create label using validated rate ID
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

            int costInCents = (int)(selectedRate.Amount * 100);

            try
            {
                // Begin database transaction - ensures both shipment and invoice are created or neither is
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Add the shipment but don't save changes yet
                        _context.Shipments.Add(shipment);
                        await _context.SaveChangesAsync();

                        // Now create the invoice
                        var invoice = new UserInvoice
                        {
                            Shipment = shipment,
                            ShipmentId = shipment.ShipmentID,
                            InvoiceAmount = costInCents,
                            DateCreated = DateTime.UtcNow,
                            DateDue = DateTime.UtcNow.AddDays(7),
                            IsPaid = false,
                            UserId = user.Id,
                            User = user,
                            PaymentReference = "",
                            Description = $"Invoice Amount {costInCents} created due to shipment {shipment.ShipmentID}, charged to user {user.UserName}"
                        };

                        user.AccountBalance += costInCents;

                        _context.Add(invoice);
                        int saveResult = await _context.SaveChangesAsync();

                        if (saveResult > 0)
                        {
                            // Only commit the transaction if both operations succeeded
                            await transaction.CommitAsync();

                            TempData["SuccessMessage"] = "Shipment created successfully with tracking number: " + shipment.TrackingNumber + ". Your account was billed a new invoice";
                            await _logService.LogUserActivityAsync(userId, $"Created shipment (ID: {shipment.ShipmentID})", GetClientIp());
                        }
                        else
                        {
                            // If invoice save failed, roll back the transaction
                            await transaction.RollbackAsync();
                            TempData["Error"] = "Unable to charge user with Selected Rate: $" + selectedRate.Amount + ". No shipment was created and your account was not charged.";
                            await _logService.LogUserActivityAsync(userId, "Failed to create invoice for shipment.", GetClientIp());
                        }
                    }
                    catch (Exception ex)
                    {
                        // Roll back the transaction if any operation failed
                        await transaction.RollbackAsync();
                        throw; // Re-throw to be caught by outer try-catch
                    }
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
                await _logService.LogUserActivityAsync(userId, $"Error creating shipment or invoice: {ex.Message}", GetClientIp());
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
            bool isAdmin = await IsUserAdmin();
            var user = await _userManager.GetUserAsync(User);
            string userId = user?.Id ?? "Unknown";
            string userName = user?.UserName ?? "Unknown";
            string ipAddress = GetClientIp();


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
                await _logService.LogUserActivityAsync(userId, $"Shipment (ID: {shipment.ShipmentID}) cancelled successfully.", GetClientIp());
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

        [HttpPost]
        public async Task<IActionResult> ValidateAddress(string street, string city = "", string state = "", string zip = "", string country = "")
        {
            try
            {
                // Create address data - only require street1 for validation
                var addressData = new Dictionary<string, object>
                {
                    { "street1", street },
                    { "validate", true },
                    { "country", string.IsNullOrEmpty(country) ? "US" : country }
                };

                // Add optional fields only if provided and not empty
                if (!string.IsNullOrEmpty(city)) addressData.Add("city", city);
                if (!string.IsNullOrEmpty(state)) addressData.Add("state", state);
                if (!string.IsNullOrEmpty(zip)) addressData.Add("zip", zip);

                // Call Shippo API to validate address
                var validation = await _shippoService.ValidateAddressAsync(addressData);

                // Return validation results and suggestions
                return Json(new
                {
                    isValid = validation.IsValid,
                    suggestions = validation.Suggestions
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Address validation error: " + ex.Message);
                return Json(new { error = ex.Message, suggestions = new List<object>() });
            }
        }

    }
}
