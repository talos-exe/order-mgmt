using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OrderMgmtRevision.Models;
using OrderMgmtRevision.Data;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;

[Authorize]
public class DashboardController : Controller
{
    private readonly IStringLocalizer<DashboardController> _localizer;
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;

    public DashboardController(IStringLocalizer<DashboardController> localizer, AppDbContext context, UserManager<User> userManager)
    {
        _localizer = localizer;
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        string userName = currentUser.UserName;

        var userShipments = _context.Shipments.Where(s => s.CreatedBy == userName).ToList();
        var userProducts = _context.Products.Where(p => p.CreatedBy == userName || p.CreatedBy == "System").ToList();

        var userShipmentsActive = userShipments.Count(s => s.Status == "CREATED");

        var mostShippedProduct = _context.Shipments
                                .Where(s => (s.CreatedBy == userName) && s.Status == "CREATED")
                                .GroupBy(s => s.Product.ProductName)
                                .Select(s => new { ProductName = s.Key, Count = s.Count() })
                                .FirstOrDefault();

        var activelyShippedProductCount = _context.Shipments
                                            .Where(s => s.CreatedBy == userName && s.Status == "CREATED")
                                            .Select(s => s.Product.ProductName)
                                            .Distinct()
                                            .Count();

        var activeInvoices = _context.UserInvoices
                            .Where(i => i.User == currentUser && i.IsPaid == false)
                            .Count();

        var mostPopularCarrier = userShipments
                            .GroupBy(s => s.Rate.Provider)
                            .OrderByDescending(g => g.Count())
                            .Select(g => new { Provider = g.Key, Count = g.Count() })
                            .FirstOrDefault();

        var workOrdersToReview = _context.WorkOrders
                            .Where(w => w.CreatedBy == userName)
                            .Count();


        var summaryData = new List<SummaryItem>
        {
            new SummaryItem
            {
                Title = _localizer["Product"],
                LabelValues = new List<LabelValue>
                {
                    new LabelValue { Label = _localizer["Most Shipped Product"], Value = mostShippedProduct?.ProductName ?? "N/A" }
                }
            },
            new SummaryItem
            {
                Title = _localizer["Outbound"],
                LabelValues = new List<LabelValue>
                {
                    new LabelValue { Label = _localizer["Shipments Outbound"], Value = userShipmentsActive.ToString() }
                }
            },
            new SummaryItem
            {
                Title = _localizer["Total Products"],
                LabelValues = new List<LabelValue>
                {
                    new LabelValue { Label = _localizer["Actively Shipped"], Value = activelyShippedProductCount.ToString() }
                }
            },
            new SummaryItem
            {
                Title = _localizer["Invoices"],
                LabelValues = new List<LabelValue>
                {
                    new LabelValue { Label = _localizer["Active Invoices"], Value = activeInvoices.ToString() }
                }
            },
            new SummaryItem
            {
                Title = _localizer["Most Popular Carrier"],
                LabelValues = new List<LabelValue>
                {
                    new LabelValue { Label = _localizer["Carrier Name"], Value = mostPopularCarrier?.Provider  ?? "N/A" }
                }
            },
            new SummaryItem
            {
                Title = _localizer["Work Order"],
                LabelValues = new List<LabelValue>
                {
                    new LabelValue { Label = _localizer["To Be Reviewed"], Value = workOrdersToReview.ToString() }
                }
            }
        };

        var pieChartData = new
        {
            Labels = new string[] { "Red, Blue, Yellow" },
            Data = new int[] { 30, 14, 6 },
            BackgroundColor = new string[] { "#FF0000", "#0000FF", "#FFFF00" }
        };

        ViewData["SummaryData"] = summaryData;
        ViewData["PieChartData"] = pieChartData;

        return View();
    }
}

