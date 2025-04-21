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

// Get connection string from environment variable (Azure or local)
var connectionString = Environment.GetEnvironmentVariable("OrderMgmtApp_ConnectionString") ??
                        builder.Configuration.GetConnectionString("DefaultConnection");

var shippoApiKey = Environment.GetEnvironmentVariable("SHIPPO_API_KEY") ??
    builder.Configuration["Shippo:ApiKey"];

var stripeConnectionString = Environment.GetEnvironmentVariable("STRIPE_CONNECTION_STRING") ??
    builder.Configuration["Stripe:ConnectionString"];

var stripeConfig = stripeConnectionString?
    .Split(';', StringSplitOptions.RemoveEmptyEntries)
    .Select(part => part.Split('=', 2))
    .ToDictionary(split => split[0].Trim(), split => split.Length > 1 ? split[1].Trim() : "");

var emailSettingsConnectionString = Environment.GetEnvironmentVariable("EMAILSETTINGS_CONNECTION_STRING") ??
                                     builder.Configuration["EmailSettings:ConnectionString"];

var emailSettings = emailSettingsConnectionString?
    .Split(';', StringSplitOptions.RemoveEmptyEntries)
    .Select(part => part.Split('=', 2))
    .ToDictionary(split => split[0].Trim(), split => split.Length > 1 ? split[1].Trim() : "");


// Register AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Bind Shippo to IConfiguration
builder.Configuration["Shippo:ApiKey"] = shippoApiKey ?? "";

builder.Configuration["Stripe:PublishableKey"] = stripeConfig?.GetValueOrDefault("PublishableKey", "") ?? "";
builder.Configuration["Stripe:SecretKey"] = stripeConfig?.GetValueOrDefault("SecretKey", "") ?? "";

// Set into IConfiguration for DI
builder.Configuration["EmailSettings:Sender"] = emailSettings?.GetValueOrDefault("Sender", "") ?? "";
builder.Configuration["EmailSettings:Password"] = emailSettings?.GetValueOrDefault("Password", "") ?? "";
builder.Configuration["EmailSettings:SmtpServer"] = emailSettings?.GetValueOrDefault("SmtpServer", "") ?? "";
builder.Configuration["EmailSettings:Port"] = emailSettings?.GetValueOrDefault("Port", "587") ?? "587";

// Register Identity services
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
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
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddSingleton<FedExService>();
builder.Services.AddHttpClient<ShippoService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddHttpClient<StripeService>();
builder.Services.AddSession();

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
app.UseSession();
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
