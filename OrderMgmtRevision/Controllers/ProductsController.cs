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


        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

            var user = await _userManager.GetUserAsync(User);
            string userName = user?.UserName ?? "Unknown";
            bool isAdmin = await IsUserAdmin();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SKUSortParm = sortOrder == "SKU" ? "sku_desc" : "SKU";
            ViewBag.StockSortParm = sortOrder == "Stock" ? "stock_desc" : "Stock";
            ViewBag.CreatedBySortParm = sortOrder == "CreatedBy" ? "createdby_desc" : "CreatedBy";
            ViewBag.CreatedAtSortParm = sortOrder == "createdat_desc" ? "CreatedAt" : "createdat_desc";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var products = _dbContext.Products.AsQueryable();
            if (!isAdmin) { products = products.Where(p => p.CreatedBy == userName); }

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
                case "CreatedAt":
                    products = products.OrderBy(p => p.CreatedAt);
                    break;
                case "createdat_desc":
                    products = products.OrderByDescending(p => p.CreatedAt);
                    break;
                default:
                    products = products.OrderBy(p => p.ProductName); // default sort
                    break;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);

            ViewBag.TotalProducts = await products.CountAsync();
            ViewBag.TotalShipped = await products.SumAsync(p => p.ShipAmount ?? 0);

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

            bool skuExists = await _dbContext.Products.AnyAsync(p => p.SKU == model.SKU);
            bool nameExists = await _dbContext.Products.AnyAsync(p => p.ProductName == model.ProductName);

            if (skuExists || nameExists)
            {
                if (skuExists)
                {
                    TempData["Error"] = $"Product with SKU '{model.SKU}' already exists.";
                    logMessage = isAdmin
                        ? $"[Administrator] Product creation failed: SKU '{model.SKU}' already exists."
                        : $"Product creation failed: SKU '{model.SKU}' already exists.";
                }

                if (nameExists)
                {
                    TempData["Error"] = $"Product with name '{model.ProductName}' already exists.";
                    logMessage = isAdmin
                        ? $"[Administrator] Product creation failed: Product name '{model.ProductName}' already exists."
                        : $"Product creation failed: Product name '{model.ProductName}' already exists.";
                }

                await _logService.LogUserActivityAsync(userId, logMessage, ipAddress);
                return RedirectToAction("Index", "Products");
            }


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

            user.ProductsActive++;
            user.ProductsTotal++;

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
        }

        [HttpGet]
        public async Task<IActionResult> _EditProduct(string productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }
            return PartialView("_EditProduct", product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(Product model)
        {
            bool isAdmin = await IsUserAdmin();
            var user = await _userManager.GetUserAsync(User);
            string userId = user?.Id ?? "Unknown";
            string userName = user?.UserName ?? "Unknown";
            string ipAddress = GetClientIp();
            string logMessage = "";

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = "Invalid model state. Errors: " + string.Join(" | ", errors);
                logMessage = isAdmin ? $"[Administrator] Failed product edit; invalid model state." : $"Failed product edit; invalid model state.";
                await _logService.LogUserActivityAsync(userId, logMessage, ipAddress);
                return RedirectToAction("Index", "Products");
            }

            var existingProduct = await _dbContext.Products.FindAsync(model.ProductID);
            if (existingProduct == null)
            {
                TempData["Error"] = "Product not found.";
                return RedirectToAction("Index", "Products");
            }

            // Update fields
            existingProduct.ProductName = model.ProductName;
            existingProduct.Description = model.Description;
            existingProduct.SKU = model.SKU;
            existingProduct.Price = model.Price;
            existingProduct.Cost = model.Cost;
            existingProduct.Stock = model.Stock;
            existingProduct.Weight = model.Weight;
            existingProduct.Width = model.Width;
            existingProduct.Length = model.Length;
            existingProduct.Height = model.Height;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            try
            {
                _dbContext.Products.Update(existingProduct);
                await _dbContext.SaveChangesAsync();
                TempData["SuccessMessage"] = "Product updated successfully.";
                logMessage = isAdmin
                    ? $"[Administrator] {userName} edited product {existingProduct.ProductName} | SKU: {existingProduct.SKU}"
                    : $"User {userName} edited product {existingProduct.ProductName} | SKU: {existingProduct.SKU}";
                await _logService.LogUserActivityAsync(userId, logMessage, ipAddress);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while updating the product.";
                await _logService.LogUserActivityAsync(userId, $"Failed to update product. Error: {ex.Message}", ipAddress);
            }

            return RedirectToAction("Index", "Products");
        }

        [HttpGet]
        public async Task<IActionResult> _DeleteProduct(string productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }
            return PartialView("_DeleteProduct", product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(string productId)
        {
            bool isAdmin = await IsUserAdmin();
            var user = await _userManager.GetUserAsync(User);
            string userId = user?.Id ?? "Unknown";
            string userName = user?.UserName ?? "Unknown";
            string ipAddress = GetClientIp();
            string logMessage = "";

            try
            {
                var product = await _dbContext.Products.FindAsync(productId);
                if (product == null)
                {
                    TempData["Error"] = "Product not found.";
                    return RedirectToAction("Index", "Products");
                }

                string productName = product.ProductName;
                string productSKU = product.SKU;

                user.ProductsActive--;

                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Product deleted successfully.";
                logMessage = isAdmin
                    ? $"[Administrator] {userName} deleted product {productName} | SKU: {productSKU}"
                    : $"User {userName} deleted product {productName} | SKU: {productSKU}";

                await _logService.LogUserActivityAsync(userId, logMessage, ipAddress);
                return RedirectToAction("Index", "Products");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the product.";
                await _logService.LogUserActivityAsync(userId, $"Failed to delete product. Error: {ex.Message}", ipAddress);
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

        [HttpGet]
        public async Task<IActionResult> GetProductDetailsForShipment(string productId)
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

            return Json(productViewModel);
        }
    }
}
