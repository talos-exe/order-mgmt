using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Identity;
using OrderMgmtRevision.Data;
using OrderMgmtRevision.Models;
using OrderMgmtRevision.Config;
using System.Globalization;
using System.Linq;
using OrderMgmtRevision.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment.EnvironmentName;

var fedexConnectionString = Environment.GetEnvironmentVariable("FEDEX_CONNECTION_STRING") ??
    builder.Configuration["FedEx:ConnectionString"];

var connectionString = Environment.GetEnvironmentVariable("OrderMgmtApp_ConnectionString") ??
                        builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

var fedexConfig = fedexConnectionString?
    .Split(';', StringSplitOptions.RemoveEmptyEntries)
    .Select(part => part.Split('=', 2))
    .ToDictionary(split => split[0].Trim(), split => split.Length > 1 ? split[1].Trim() : "");

builder.Configuration["FedEx:ApiKey"] = fedexConfig?.GetValueOrDefault("ApiKey", "") ?? "";
builder.Configuration["FedEx:ApiSecret"] = fedexConfig?.GetValueOrDefault("ApiSecret", "") ?? "";
builder.Configuration["FedEx:AccountNumber"] = fedexConfig?.GetValueOrDefault("AccountNumber", "") ?? "";
builder.Configuration["FedEx:LtlShipperAccountNumber"] = fedexConfig?.GetValueOrDefault("LtlShipperAccountNumber", "") ?? "";
builder.Configuration["FedEx:BaseUrl"] = fedexConfig?.GetValueOrDefault("BaseUrl", "") ?? "";

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// ✅ Localization setup
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("zh") };
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new CookieRequestCultureProvider()
    };
});

builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.AddRazorPages();
builder.Services.AddTransient<DataSeeder>();
builder.Services.AddSingleton<FedExService>();
builder.Services.AddScoped<ILogService, LogService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await dataSeeder.SeedDataAsync();

    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
}

// ✅ Apply Request Localization Middleware
var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Dashboard}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

app.Run();
