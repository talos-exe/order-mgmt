using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Text.Json;

public class DashboardController : Controller
{
    private readonly IStringLocalizer<DashboardController> _localizer;

    public DashboardController(IStringLocalizer<DashboardController> localizer)
    {
        _localizer = localizer;
    }

    [Authorize]
    public IActionResult Index()
    {
        var summaryData = new List<SummaryItem>
        {
            new SummaryItem
            {
                Title = _localizer["Inbound"],
                LabelValues = new List<LabelValue>
                {
                    new LabelValue { Label = _localizer["To Be Received"], Value = 0 }
                }
            },
            new SummaryItem
            {
                Title = _localizer["Outbound"],
                LabelValues = new List<LabelValue>
                {
                    new LabelValue { Label = _localizer["Dropshipping"], Value = 0 },
                    new LabelValue { Label = _localizer["Stock Transfer"], Value = 0 }
                }
            },
            new SummaryItem
            {
                Title = _localizer["Returns"],
                LabelValues = new List<LabelValue>
                {
                    new LabelValue { Label = _localizer["To Be Received"], Value = 0 }
                }
            },
            new SummaryItem
            {
                Title = _localizer["FBA Returns"],
                LabelValues = new List<LabelValue>
                {
                    new LabelValue { Label = _localizer["To Be Received"], Value = 0 },
                    new LabelValue { Label = _localizer["To Be Shipped"], Value = 0 }
                }
            },
            new SummaryItem
            {
                Title = _localizer["Transshipment"],
                LabelValues = new List<LabelValue>
                {
                    new LabelValue { Label = _localizer["To Be Received"], Value = 0 }
                }
            },
            new SummaryItem
            {
                Title = _localizer["Work Order"],
                LabelValues = new List<LabelValue>
                {
                    new LabelValue { Label = _localizer["To Be Reviewed"], Value = 0 }
                }
            },
            new SummaryItem
            {
                Title = _localizer["Order Cutoff"],
                LabelValues = new List<LabelValue>
                {
                    new LabelValue { Label = _localizer["To Be Processed"], Value = 0 }
                }
            }
        };

        return View(summaryData);
    }
}

