using Microsoft.EntityFrameworkCore;
using OrderMgmtRevision.Data;
using OrderMgmtRevision.Models; // Add this if missing

var builder = WebApplication.CreateBuilder(args);

// Register AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 🔹 Run Database Migrations and Seed Data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate(); // Apply migrations

    // Check if data already exists before seeding
    if (!context.Warehouses.Any())
    {
        context.Warehouses.AddRange(
            new Warehouse {WarehouseName = "Main Warehouse", Address = "111 New York" },
            new Warehouse {WarehouseName = "Backup Warehouse", Address = "444 Los Angeles" }
        );

        context.SaveChanges();
    }

    if (!context.Products.Any())
    {
        context.Products.AddRange(
            new Product {ProductName = "Laptop", Description = "ASUS Laptop", SKU = "LAP-123", Price = 999.99m, Cost = 700.00m },
            new Product {ProductName = "Mouse", Description = "HP Mouse", SKU = "HPMOUSE-001", Price = 49.99m, Cost = 20.00m }
        );

        context.SaveChanges();
    }

    if (!context.InventoryAll.Any())
    {
        context.InventoryAll.AddRange(
            new Inventory {ProductID = 1, WarehouseID = 1, Quantity = 30 },
            new Inventory {ProductID = 2, WarehouseID = 1, Quantity = 10 },
            new Inventory {ProductID = 1, WarehouseID = 2, Quantity = 50 }
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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();