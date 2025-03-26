using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Identity;
using OrderMgmtRevision.Data;
using OrderMgmtRevision.Models;
using OrderMgmtRevision.Config;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);

// Determine environment
var environment = builder.Environment.EnvironmentName;

// Get FedEx Credentials From Environment Variable
var fedexConnectionString = Environment.GetEnvironmentVariable("FEDEX_CONNECTION_STRING") ??
    builder.Configuration["FedEx:ConnectionString"]; // fallback to appsettings.json

// Get connection string from environment variable (Azure or local)
var connectionString = Environment.GetEnvironmentVariable("OrderMgmtApp_ConnectionString") ??
                        builder.Configuration.GetConnectionString("DefaultConnection");

// Register AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Parse FedEx connection string
var fedexConfig = fedexConnectionString?
    .Split(';', StringSplitOptions.RemoveEmptyEntries)
    .Select(part => part.Split('=', 2))
    .ToDictionary(split => split[0].Trim(), split => split.Length > 1 ? split[1].Trim() : "");

// Bind to IConfiguration
builder.Configuration["FedEx:ApiKey"] = fedexConfig?.GetValueOrDefault("ApiKey", "") ?? "";
builder.Configuration["FedEx:ApiSecret"] = fedexConfig?.GetValueOrDefault("ApiSecret", "") ?? "";
builder.Configuration["FedEx:AccountNumber"] = fedexConfig?.GetValueOrDefault("AccountNumber", "") ?? "";
builder.Configuration["FedEx:LtlShipperAccountNumber"] = fedexConfig?.GetValueOrDefault("LtlShipperAccountNumber", "") ?? "";
builder.Configuration["FedEx:BaseUrl"] = fedexConfig?.GetValueOrDefault("BaseUrl", "") ?? "";

// Register Identity services
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

// Configure cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// Register localization services
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Add MVC with view & data annotations localization
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.AddRazorPages();
builder.Services.AddTransient<DataSeeder>();
builder.Services.AddSingleton<FedExService>();

// Supported cultures
var supportedCultures = new[] { "en", "zh" };
var defaultCulture = "en";

// Custom Request Culture Provider (Query String Based)
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(defaultCulture)
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures)
    .AddInitialRequestCultureProvider(new CustomRequestCultureProvider(context =>
    {
        var queryLang = context.Request.Query["lang"].ToString();
        var culture = supportedCultures.Contains(queryLang) ? queryLang : defaultCulture;
        return Task.FromResult(new ProviderCultureResult(culture, culture));
    }));

var app = builder.Build();

// Run Database Migrations and Seed Data
using (var scope = app.Services.CreateScope())
{
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await dataSeeder.SeedDataAsync();

    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
}

// Use Request Localization Middleware
app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline
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
    endpoints.MapRazorPages(); // Ensure Identity pages work
});

app.Run();
