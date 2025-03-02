using Microsoft.EntityFrameworkCore;
using OrderMgmtRevision.Data;
using OrderMgmtRevision.Models;
using Microsoft.AspNetCore.Identity;
using OrderMgmtRevision.Config;

var builder = WebApplication.CreateBuilder(args);

// Determine environment
var environment = builder.Environment.EnvironmentName;

//Get FedEx Credentials From Environment Variable.
var fedexConnectionString = Environment.GetEnvironmentVariable("FEDEX_CONNECTION_STRING") ??
    builder.Configuration["FedEx:ConnectionString"]; // fallback to appsettings.json

// Get connection string from environment variable (Azure or local)
var connectionString = Environment.GetEnvironmentVariable("OrderMgmtApp_ConnectionString") ??
                        builder.Configuration.GetConnectionString("DefaultConnection");

// Register AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));


//Parse fedex connection string
var fedexConfig = fedexConnectionString?
    .Split(';', StringSplitOptions.RemoveEmptyEntries)
    .Select(part => part.Split('=', 2))
    .ToDictionary(split => split[0].Trim(), split => split.Length > 1 ? split[1].Trim() : "");

//Bind to IConfiguration
builder.Configuration["FedEx:ApiKey"] = fedexConfig?.GetValueOrDefault("ApiKey", "") ?? "";
builder.Configuration["FedEx:ApiSecret"] = fedexConfig?.GetValueOrDefault("ApiSecret", "") ?? "";
builder.Configuration["FedEx:AccountNumber"] = fedexConfig?.GetValueOrDefault("AccountNumber", "") ?? "";
builder.Configuration["FedEx:LtlShipperAccountNumber"] = fedexConfig?.GetValueOrDefault("LtlShipperAccountNumber", "") ?? "";
builder.Configuration["FedEx:BaseUrl"] = fedexConfig?.GetValueOrDefault("BaseUrl", "") ?? "";

// Register FedEx settings for dependency injection
//builder.Services.Configure<FedExSettings>(builder.Configuration.GetSection("FedEx"));
// ENABLE THIS FOR STRONG-TYPED ACCESS.


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

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddTransient<DataSeeder>();

// FedEx Service
builder.Services.AddSingleton<FedExService>();

var app = builder.Build();

// Run Database Migrations and Seed Data
using (var scope = app.Services.CreateScope())
{
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await dataSeeder.SeedDataAsync();

    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
}



//Configure HTTP request pipeline.
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
    // MVC controller routes
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Dashboard}/{action=Index}/{id?}");
    // Razor Pages for Identity
    endpoints.MapRazorPages(); // This ensures Identity pages work
});

app.MapRazorPages();
app.Run();