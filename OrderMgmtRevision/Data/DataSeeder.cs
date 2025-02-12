using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrderMgmtRevision.Models;

namespace OrderMgmtRevision.Data
{
    public class DataSeeder
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public DataSeeder(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task SeedDataAsync()
        {
            // Seed roles
            var roles = new[] { "Admin", "User", "Manager" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed Admin user if not exists
            var adminUser = await _userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    FullName = "Admin User"
                };

                var result = await _userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin"); // Assign Admin role to the user
                }
            }

            var guestAccount = await _userManager.FindByNameAsync("User1");
            if (guestAccount == null)
            {
                guestAccount = new User
                {
                    UserName = "User1",
                    Email = "user@user.com",
                    FullName = "User Account"
                };

                var result = await _userManager.CreateAsync(guestAccount, "User@123");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(guestAccount, "User"); // Assign Admin role to the user
                }
            }

            // Seed other data if it doesn't exist
            if (!_context.Warehouses.Any())
            {
                _context.Warehouses.AddRange(
                    new Warehouse { WarehouseName = "Main Warehouse", Address = "111 New York" },
                    new Warehouse { WarehouseName = "Backup Warehouse", Address = "444 Los Angeles" }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.Products.Any())
            {
                _context.Products.AddRange(
                    new Product { ProductName = "Laptop", Description = "ASUS Laptop", SKU = "LAP-123", Price = 999.99m, Cost = 700.00m },
                    new Product { ProductName = "Mouse", Description = "HP Mouse", SKU = "HPMOUSE-001", Price = 49.99m, Cost = 20.00m }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.InventoryAll.Any())
            {
                _context.InventoryAll.AddRange(
                    new Inventory { ProductID = 1, WarehouseID = 1, Quantity = 30 },
                    new Inventory { ProductID = 2, WarehouseID = 1, Quantity = 10 },
                    new Inventory { ProductID = 1, WarehouseID = 2, Quantity = 50 }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.Shipments.Any())
            {
                _context.Shipments.AddRange(
                    new Shipment { ProductID = 1, Quantity = 5, SourceWarehouseID = 1, DestinationWarehouseID = 2, Cost = 30.00m, TrackingNumber = "111" },
                    new Shipment { ProductID = 2, Quantity = 2, SourceWarehouseID = 2, DestinationWarehouseID = 1, Cost = 30.00m, TrackingNumber = "112" }
                );
                await _context.SaveChangesAsync();
            }
        }
    }
}