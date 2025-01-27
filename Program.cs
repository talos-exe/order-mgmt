using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data; // Add this for your DbContext
using OrderManagementSystem.Models; // Add this for your models

var builder = WebApplication.CreateBuilder(args);

// Set the DataDirectory for the application
AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(builder.Environment.ContentRootPath, "App_Data"));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Load the FedEx settings from appsettings.json
builder.Services.Configure<FedExSettings>(builder.Configuration.GetSection("FedExApi"));

// Register the DbContext with Dependency Injection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Ensure the connection string is correct

// Register HttpClient and the FedEx services
builder.Services.AddHttpClient<FedExAuthService>();
builder.Services.AddHttpClient<FedExShippingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Define routing for general views and FedEx API integration
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Add additional routes if necessary

app.Run();