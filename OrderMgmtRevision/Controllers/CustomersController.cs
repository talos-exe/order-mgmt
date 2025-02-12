using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderMgmtRevision.Models;

namespace OrderMgmtRevision.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}