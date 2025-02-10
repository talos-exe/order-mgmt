using Microsoft.EntityFrameworkCore;
using OrderMgmtRevision.Data;
using OrderMgmtRevision.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);

// Register AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
    options.LoginPath = "/Views/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";

});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Run Database Migrations and Seed Data
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "User", "Manager" };
    
    foreach (var role in roles )
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Check if data already exists before seeding
    if (!context.Warehouses.Any())
    {
        context.Warehouses.AddRange(
            new Warehouse { WarehouseName = "Main Warehouse", Address = "111 New York" },
            new Warehouse { WarehouseName = "Backup Warehouse", Address = "444 Los Angeles" }
        );

        context.SaveChanges();
    }

    if (!context.Products.Any())
    {
        context.Products.AddRange(
            new Product { ProductName = "Laptop", Description = "ASUS Laptop", SKU = "LAP-123", Price = 999.99m, Cost = 700.00m },
            new Product { ProductName = "Mouse", Description = "HP Mouse", SKU = "HPMOUSE-001", Price = 49.99m, Cost = 20.00m }
        );

        context.SaveChanges();
    }

    if (!context.InventoryAll.Any())
    {
        context.InventoryAll.AddRange(
            new Inventory { ProductID = 1, WarehouseID = 1, Quantity = 30 },
            new Inventory { ProductID = 2, WarehouseID = 1, Quantity = 10 },
            new Inventory { ProductID = 1, WarehouseID = 2, Quantity = 50 }
        );

        context.SaveChanges();
    }

    if (!context.Shipments.Any())
    {
        context.Shipments.AddRange(
            new Shipment { ProductID = 1, Quantity = 5, SourceWarehouseID = 1, DestinationWarehouseID = 2, Cost = 30.00m, TrackingNumber = "111" },
            new Shipment { ProductID = 2, Quantity = 2, SourceWarehouseID = 2, DestinationWarehouseID = 1, Cost = 30.00m, TrackingNumber = "112" }
        );
        context.SaveChanges();
    }
}




// Configure the HTTP request pipeline.
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
        pattern: "{controller=Home}/{action=Index}/{id?}");

    // Razor Pages for Identity
    endpoints.MapRazorPages(); // This ensures Identity pages work
});

app.MapRazorPages();
app.Run();