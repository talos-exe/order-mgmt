using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Models;
using System.Diagnostics;

namespace OrderManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Products()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            // Creating the ErrorViewModel without attempting to set ShowRequestId
            return View(new ErrorViewModel { RequestId = requestId });
        }
    }
}