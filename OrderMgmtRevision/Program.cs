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

// Determine environment
var environment = builder.Environment.EnvironmentName;

// Get FedEx Credentials From Environment Variable
var fedexConnectionString = Environment.GetEnvironmentVariable("FEDEX_CONNECTION_STRING") ??
    builder.Configuration["FedEx:ConnectionString"]; // fallback to appsettings.json

// Get connection string from environment variable (Azure or local)
var connectionString = Environment.GetEnvironmentVariable("OrderMgmtApp_ConnectionString") ??
                        builder.Configuration.GetConnectionString("DefaultConnection");

var shippoApiKey = Environment.GetEnvironmentVariable("SHIPPO_API_KEY") ??
    builder.Configuration["Shippo:ApiKey"];

var stripeSecretKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY") ??
    builder.Configuration["Stripe:SecretKey"];

// Register AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

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

// Bind Shippo to IConfiguration
builder.Configuration["Shippo:ApiKey"] = shippoApiKey ?? "";

builder.Configuration["Stripe:SecretKey"] = stripeSecretKey ?? "";

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

// Custom Request Culture Provider (Query String Based)
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en"),
        new CultureInfo("zh")
    };

    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    // Add the standard providers in the correct order
    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        // 1. First check for cookie
        new CookieRequestCultureProvider(),
        // 2. Then check query string
        new QueryStringRequestCultureProvider(),
        // 3. Then check Accept-Language header
        new AcceptLanguageHeaderRequestCultureProvider()
    };
});


builder.Services.AddRazorPages();
builder.Services.AddTransient<DataSeeder>();
builder.Services.AddSingleton<FedExService>();
builder.Services.AddHttpClient<ShippoService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddHttpClient<StripeService>();

var app = builder.Build();

// Run Database Migrations and Seed Data
using (var scope = app.Services.CreateScope())
{
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await dataSeeder.SeedDataAsync();

    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
}

// Use Request Localization Middleware
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);


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
