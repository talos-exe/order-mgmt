using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OrderMgmtRevision.Models;

namespace OrderMgmtRevision.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
        var summaryData = new List<SummaryItem>
        {
            new SummaryItem
            {
                Title = "Inbound",
                LabelValues = new List<LabelValue>
                {
                    new LabelValue { Label = "To Be Received", Value = 0 }
                }
            },
            new SummaryItem
            {
                Title = "Outbound",
                LabelValues = new List<LabelValue>
                {
                    new LabelValue { Label = "Dropshipping", Value = 0 },
                    new LabelValue { Label = "Stock Transfer", Value = 0 }
                }
            },
            new SummaryItem
            {
                Title = "Returns",
                LabelValues = new List<LabelValue>
                {
                    new LabelValue { Label = "To Be Received", Value = 0 }
                }
            },
            new SummaryItem
            {
                Title = "FBA Returns",
                LabelValues = new List<LabelValue>
                {
                    new LabelValue { Label = "To Be Received", Value = 0 },
                    new LabelValue { Label = "To Be Shipped", Value = 0 }
                }
            },
            new SummaryItem
            {
                Title = "Transshipment",
                LabelValues = new List<LabelValue>
                {
                    new LabelValue { Label = "To Be Received", Value = 0 }
                }
            },
            new SummaryItem
            {
                Title = "Work Order",
                LabelValues = new List<LabelValue>
                {
                    new LabelValue { Label = "To Be Reviewed", Value = 0 }
                }
            },
            new SummaryItem
            {
                Title = "Order Cutoff",
                LabelValues = new List<LabelValue>
                {
                    new LabelValue { Label = "To Be Processed", Value = 0 }
                }
            }
        };

        return View(summaryData);
        }

    }
}