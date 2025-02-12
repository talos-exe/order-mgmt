using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OrderMgmtRevision.Models;

namespace OrderMgmtRevision.Controllers
{
    public class CustomersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}