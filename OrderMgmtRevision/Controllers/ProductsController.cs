using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderMgmtRevision.Models;

namespace OrderMgmtRevision.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
          var productData = new List<Product>
          {
            new Product
            {
              SKU = "00054-3010-1420",
              ProductName = "Test Product 1",
              Price = 49.99m,
              Cost = 5.00m
            },
            new Product
            {
              SKU = "00054-3010-1410",
              ProductName = "Test Product 2",
              Price = 29.99m
            },
            new Product
            {
              SKU = "00054-3010-1400",
              ProductName = "Test Product 3",
              Price = 85.50m,
              Cost = 12.34m
            }
          };
          return View(productData);
        }

    }
}
