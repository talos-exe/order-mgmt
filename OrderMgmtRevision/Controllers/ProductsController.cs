using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderMgmtRevision.Data;
using OrderMgmtRevision.Models;
using OrderMgmtRevision.Services;

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

            // Get the last product ID created for today
            var lastProduct = _dbContext.Products
                .Where(p => p.ProductID.StartsWith($"PROD-{datePart}-"))
                .OrderByDescending(p => p.ProductID)
                .FirstOrDefault();

            // If no product exists for today, start from 001
            string newProductNumber = "001";
            if (lastProduct != null)
            {
                // Extract the number part from the last product's ID (e.g., 001 from PROD-20250402-001)
                var lastProductNumber = lastProduct.ProductID.Substring(12);

                // Increment the last number to create a new sequential number
                int newProductInt = int.Parse(lastProductNumber) + 1;
                newProductNumber = newProductInt.ToString("D3"); // Ensure 3 digits (e.g., 001, 002, etc.)
            }

            // Combine the parts to form the new ProductID
            return $"PROD-{datePart}-{newProductNumber}";
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


        public async Task<IActionResult> Index()
        {
            // Enable for debug info.
            //var productData = new List<Product>
            //{
            //  new Product
            //  {
            //    SKU = "00054-3010-1420",
            //    ProductName = "Test Product 1",
            //    Price = 49.99m,
            //    Cost = 5.00m
            //  },
            //  new Product
            //  {
            //    SKU = "00054-3010-1410",
            //    ProductName = "Test Product 2",
            //    Price = 29.99m
            //  },
            //  new Product
            //  {
            //    SKU = "00054-3010-1400",
            //    ProductName = "Test Product 3",
            //    Price = 85.50m,
            //    Cost = 12.34m
            //  }
            //};
            //return View(productData);

            var productList = await _dbContext.Products.ToListAsync();

            return View(productList);
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
