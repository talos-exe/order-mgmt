using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderMgmtRevision.Data;
using OrderMgmtRevision.Models;
using OrderMgmtRevision.Services;
using X.PagedList.Extensions;

namespace OrderMgmtRevision.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly ILogService _logService;

        public ProductsController(UserManager<User> userManager, AppDbContext dbContext, ILogService logService)
        { 
            _userManager = userManager;
            _dbContext = dbContext;
            _logService = logService;
        }

        public string GenerateProductId()
        {
            // Get the current date in the format "yyyyMMdd"
            string datePart = DateTime.Now.ToString("yyyyMMdd");
            string prefix = $"PROD-{datePart}-";

            // Get the last product ID created for today
            var lastProduct = _dbContext.Products
                .Where(p => p.ProductID.StartsWith(prefix))
                .OrderByDescending(p => p.ProductID)
                .FirstOrDefault();

            // If no product exists for today, start from 001
            string newProductNumber = "001";

            if (lastProduct != null)
            {
                try
                {
                    // Extract the number part from the last product's ID (e.g., 001 from PROD-20250402-001)
                    var lastProductNumber = lastProduct.ProductID.Substring(prefix.Length);

                    // Make sure we only have digits
                    if (int.TryParse(lastProductNumber, out int lastNumber))
                    {
                        // Increment the last number to create a new sequential number
                        int newProductInt = lastNumber + 1;
                        newProductNumber = newProductInt.ToString("D3"); // Ensure 3 digits
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception for debugging
                    System.Diagnostics.Debug.WriteLine($"Error parsing product ID: {ex.Message}");
                    // Continue with default "001"
                }
            }

            // Combine the parts to form the new ProductID
            return $"{prefix}{newProductNumber}";
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


        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SKUSortParm = sortOrder == "SKU" ? "sku_desc" : "SKU";
            ViewBag.StockSortParm = sortOrder == "Stock" ? "stock_desc" : "Stock";
            ViewBag.CreatedBySortParm = sortOrder == "CreatedBy" ? "createdby_desc" : "CreatedBy";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var products = from p in _dbContext.Products
                           select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ProductName.Contains(searchString)
                                            || p.SKU.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.ProductName);
                    break;
                case "SKU":
                    products = products.OrderBy(p => p.SKU);
                    break;
                case "sku_desc":
                    products = products.OrderByDescending(p => p.SKU);
                    break;
                case "Stock":
                    products = products.OrderBy(p => p.Stock);
                    break;
                case "stock_desc":
                    products = products.OrderByDescending(p => p.Stock);
                    break;
                case "CreatedBy":
                    products = products.OrderBy(p => p.CreatedBy);
                    break;
                case "createdby_desc":
                    products = products.OrderByDescending(p => p.CreatedBy);
                    break;
                default:
                    products = products.OrderBy(p => p.ProductName); // default sort
                    break;
            }

            int pageSize = 15;
            int pageNumber = (page ?? 1);

            return View(products.ToPagedList(pageNumber, pageSize));
        }

        private string GetClientIp()
        {
            return HttpContext.Connection.RemoteIpAddress?.ToString();
        }

        [HttpGet]
        public IActionResult _CreateProduct()
        {
            return PartialView("_CreateProduct", new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(Product model)
        {
            bool isAdmin = await IsUserAdmin();
            var user = await _userManager.GetUserAsync(User);
            string userId = user?.Id ?? "Unknown";
            string userName = user?.UserName ?? "Unknown";
            string ipAddress = GetClientIp();
            string logMessage = "";

            var product = new Product
            {
                ProductID = GenerateProductId(),
                ProductName = model.ProductName,
                Description = model.Description,
                SKU = model.SKU,
                Price = model.Price,
                Cost = model.Cost,
                Stock = model.Stock,
                Weight = model.Weight,
                Height = model.Height,
                Length = model.Length,
                Width = model.Width,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userName,
                ShipAmount = 0
            };

            // Create errors and post

            try
            {
                await _dbContext.Products.AddAsync(product);
                await _dbContext.SaveChangesAsync();
                TempData["SuccessMessage"] = "Product created successfully.";
                logMessage = isAdmin
                    ? $"[Administrator] {userName} created product {model.ProductName} | SKU: {model.SKU}"
                    : $"User {userName} created product {model.ProductName} | SKU: {model.SKU}";

                await _logService.LogUserActivityAsync(userId, logMessage, ipAddress);
                return RedirectToAction("Index", "Products");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occured while creating the product.";
                await _logService.LogUserActivityAsync(userId, $"Failed to create product. Error: {ex.Message}", ipAddress);
                return RedirectToAction("Index", "Products");
            }


            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

                TempData["Error"] = "Invalid model state. Errors: " + string.Join(" | ", errors);
                logMessage = isAdmin ? $"[Administrator] Failed product creation; invalid model state." : $"Failed product creation; invalid model state.";
                await _logService.LogUserActivityAsync(userId, logMessage, ipAddress);
                return RedirectToAction("Index", "Products");
            }


        }



        [HttpGet]
        public async Task<IActionResult> GetProductDetails(string productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var productViewModel = await _dbContext.Products
                .Where(p => p.ProductID.ToString() == productId)
                .Select(p => new Product
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    SKU = p.SKU,
                    Price = p.Price,
                    Cost = p.Cost,
                    Stock = p.Stock,
                    Height = p.Height,
                    Width = p.Width,
                    Length = p.Length,
                    Weight = p.Weight,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    CreatedBy = p.CreatedBy,
                    ShipAmount = p.ShipAmount
                })
                .FirstOrDefaultAsync();

            return PartialView("_ProductDetails", productViewModel);
        }

        //public async Task<IActionResult> EditProductAsync()

    }
}
