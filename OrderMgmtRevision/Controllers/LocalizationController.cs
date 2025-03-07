using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

public class LocalizationController : Controller
{
    public IActionResult SetLanguage(string lang, string returnUrl = "/")
    {
        var culture = new CultureInfo(lang);
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        // Ensure that the return URL includes the lang parameter
        var uri = new Uri(Request.Headers["Referer"].ToString());
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        query.Set("lang", lang);
        var newUrl = $"{uri.GetLeftPart(UriPartial.Path)}?{query}";

        return Redirect(newUrl);
    }
}
