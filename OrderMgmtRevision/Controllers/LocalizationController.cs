using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;

namespace OrderMgmtRevision.Controllers
{
    public class LocalizationController : Controller
    {
        [HttpGet]
        public IActionResult SetLanguage(string lang, string returnUrl = "/")
        {
            if (string.IsNullOrEmpty(lang))
            {
                return LocalRedirect(returnUrl);
            }

            // Set cookie for ASP.NET Core localization middleware
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            // Simply redirect back without modifying the URL
            return LocalRedirect(returnUrl);
        }
    }
}
