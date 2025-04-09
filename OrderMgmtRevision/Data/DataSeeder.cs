using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrderMgmtRevision.Controllers;
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
            var roles = new[] { "Admin", "User"};
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
                    FullName = "Administrator Account",
                    DateCreated = DateTime.UtcNow,
                    LastLogin = DateTime.UtcNow,
                    LastLoginIP = "127.0.0.1"
                };

                var result = await _userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin"); // Assign Admin role to the user
                }
            }

            var guestAccount = await _userManager.FindByNameAsync("user");
            if (guestAccount == null)
            {
                guestAccount = new User
                {
                    UserName = "user",
                    Email = "user@user.com",
                    FullName = "User Account",
                    DateCreated = DateTime.UtcNow,
                    LastLogin = DateTime.UtcNow,
                    LastLoginIP = "127.0.0.1"
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
                    new Warehouse { WarehouseName = "Arctic Warehouse", Address = "111 New York" },
                    new Warehouse { WarehouseName = "Backup Warehouse", Address = "444 Los Angeles" }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.Products.Any())
            {
                _context.Products.AddRange(
                        new Product { ProductID = "PROD-20250402-001", Description = "An elite gaming laptop. Very nice and high-end.", ProductName = "ASUS Laptop", SKU = "ASUS-LAP1", Price = 999.99m, Cost = 700.00m, Stock = 20, ShipAmount = 3},
                        new Product { ProductID = "PROD-20250402-002", Description = "A basic mouse that any user can use.", ProductName = "HP Mouse", SKU = "HP01-0001", Price = 49.99m, Cost = 20.00m, Stock = 40, ShipAmount = 5 }
                    );

                await _context.SaveChangesAsync();
            }

            // Not Fully implemented yet.
            //if (!_context.InventoryAll.Any())
            //{
            //    _context.InventoryAll.AddRange(
            //        new Inventory { ProductID = 1, WarehouseID = 1, Quantity = 30 },
            //        new Inventory { ProductID = 2, WarehouseID = 1, Quantity = 10 },
            //        new Inventory { ProductID = 1, WarehouseID = 2, Quantity = 50 }
            //    );
            //    await _context.SaveChangesAsync();
            //}

            //if (!_context.Shipments.Any())
            //{
            //    _context.Shipments.AddRange(
            //        new Shipment { ProductID = 1, Quantity = 5, SourceWarehouseID = 1, DestinationWarehouseID = 2, Cost = 30.00m, TrackingNumber = "111" },
            //        new Shipment { ProductID = 2, Quantity = 2, SourceWarehouseID = 2, DestinationWarehouseID = 1, Cost = 30.00m, TrackingNumber = "112" }
            //    );
            //    await _context.SaveChangesAsync();
            //}
        }
    }
}