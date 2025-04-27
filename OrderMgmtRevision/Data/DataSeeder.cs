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
                    LastLoginIP = "127.0.0.1",
                    EmailConfirmed = true,
                    IsActive = true,
                    IsAdmin = true
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
                    LastLoginIP = "127.0.0.1",
                    EmailConfirmed = true,
                    IsActive = true
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
                    new Warehouse { WarehouseName = "Amazon DSE2", Address = "6705 E Marginal Wy S", City = "Seattle", State = "WA", Zip = "98108", PhoneNumber = "+12064952580", CountryCode = "US", WarehouseEmail = "amazondse2@example.com"},
                    new Warehouse { WarehouseName = "eBay New York", Address = "641 6th Ave", City = "New York", State = "NY",  Zip = "10011", PhoneNumber = "+13479498308", CountryCode = "US", WarehouseEmail = "ebaynewyork@example.com" }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.Products.Any())
            {
                _context.Products.AddRange(
                        new Product { ProductID = "PROD-20250402-113", Description = "An elite gaming laptop. Very nice and high-end.", ProductName = "ASUS Laptop", SKU = "ASUS-LAP1", Price = 999.99m, Cost = 700.00m, Stock = 20, ShipAmount = 3, Height = 3, Weight = 15, Width = 8, Length = 10 },
                        new Product { ProductID = "PROD-20250402-112", Description = "A basic mouse that any user can use.", ProductName = "HP Mouse", SKU = "HP01-0001", Price = 49.99m, Cost = 20.00m, Stock = 40, ShipAmount = 5, Height = 5, Weight = 20, Width = 5, Length = 5 },
                        new Product { ProductID = "PROD-20250402-001", Description = "Great value for money.", ProductName = "Corsair RAM", SKU = "FKVC-2951", Price = 1009.56m, Cost = 739.15m, Stock = 51, ShipAmount = 4, Height = 2, Weight = 6, Width = 11, Length = 18 },
                        new Product { ProductID = "PROD-20250402-002", Description = "Great value for money.", ProductName = "Sony Xperia", SKU = "XDIR-W365", Price = 1387.49m, Cost = 1005.33m, Stock = 81, ShipAmount = 1, Height = 4, Weight = 18, Width = 5, Length = 4 },
                        new Product { ProductID = "PROD-20250402-003", Description = "Top-tier performance.", ProductName = "JBL Speaker", SKU = "NJLE-R252", Price = 71.99m, Cost = 57.29m, Stock = 70, ShipAmount = 10, Height = 6, Weight = 3, Width = 11, Length = 16 },
                        new Product { ProductID = "PROD-20250402-004", Description = "Slim and lightweight.", ProductName = "TP-Link Router", SKU = "RMLO-D413", Price = 1395.88m, Cost = 1031.91m, Stock = 74, ShipAmount = 1, Height = 2, Weight = 10, Width = 4, Length = 13 },
                        new Product { ProductID = "PROD-20250402-005", Description = "High performance for gamers.", ProductName = "AMD Ryzen", SKU = "WMPH-N502", Price = 216.85m, Cost = 148.91m, Stock = 14, ShipAmount = 10, Height = 12, Weight = 21, Width = 12, Length = 10 },
                        new Product { ProductID = "PROD-20250402-006", Description = "Built for productivity.", ProductName = "Logitech Keyboard", SKU = "XAPS-I776", Price = 106.16m, Cost = 72.26m, Stock = 48, ShipAmount = 2, Height = 7, Weight = 7, Width = 8, Length = 9 },
                        new Product { ProductID = "PROD-20250402-007", Description = "Affordable and reliable.", ProductName = "Acer Chromebook", SKU = "JOAM-A614", Price = 1070.2m, Cost = 687.56m, Stock = 17, ShipAmount = 2, Height = 10, Weight = 18, Width = 8, Length = 6 },
                        new Product { ProductID = "PROD-20250402-008", Description = "Great value for money.", ProductName = "Acer Chromebook", SKU = "KUSN-D570", Price = 745.52m, Cost = 512.7m, Stock = 35, ShipAmount = 6, Height = 9, Weight = 16, Width = 12, Length = 8 },
                        new Product { ProductID = "PROD-20250402-009", Description = "Ergonomic and durable.", ProductName = "Razer Headset", SKU = "TORQ-C075", Price = 1086.43m, Cost = 852.91m, Stock = 52, ShipAmount = 9, Height = 3, Weight = 24, Width = 15, Length = 20 },
                        new Product { ProductID = "PROD-20250402-010", Description = "Great value for money.", ProductName = "ASUS Laptop", SKU = "KCDZ-R389", Price = 1210.78m, Cost = 908.96m, Stock = 64, ShipAmount = 1, Height = 8, Weight = 2, Width = 14, Length = 8 },
                        new Product { ProductID = "PROD-20250402-011", Description = "Top-tier performance.", ProductName = "Corsair RAM", SKU = "SGBR-3547", Price = 1178.43m, Cost = 864.22m, Stock = 94, ShipAmount = 3, Height = 8, Weight = 21, Width = 6, Length = 17 },
                        new Product { ProductID = "PROD-20250402-012", Description = "Ergonomic and durable.", ProductName = "Dell Monitor", SKU = "BWNP-H135", Price = 662.43m, Cost = 451.03m, Stock = 91, ShipAmount = 5, Height = 12, Weight = 18, Width = 15, Length = 5 },
                        new Product { ProductID = "PROD-20250402-013", Description = "Ergonomic and durable.", ProductName = "Netgear Switch", SKU = "WSTC-7127", Price = 143.45m, Cost = 89.96m, Stock = 43, ShipAmount = 3, Height = 5, Weight = 7, Width = 4, Length = 7 },
                        new Product { ProductID = "PROD-20250402-014", Description = "Great value for money.", ProductName = "Acer Chromebook", SKU = "OADS-U362", Price = 677.07m, Cost = 532.4m, Stock = 30, ShipAmount = 3, Height = 6, Weight = 14, Width = 5, Length = 15 },
                        new Product { ProductID = "PROD-20250402-015", Description = "Top-tier performance.", ProductName = "MSI Motherboard", SKU = "DUMN-5129", Price = 224.5m, Cost = 168.95m, Stock = 23, ShipAmount = 3, Height = 12, Weight = 13, Width = 2, Length = 5 },
                        new Product { ProductID = "PROD-20250402-016", Description = "Cutting-edge technology inside.", ProductName = "MSI Motherboard", SKU = "NYPS-3317", Price = 1260.45m, Cost = 875.36m, Stock = 81, ShipAmount = 8, Height = 2, Weight = 19, Width = 8, Length = 12 },
                        new Product { ProductID = "PROD-20250402-017", Description = "Affordable and reliable.", ProductName = "JBL Speaker", SKU = "TFGG-V662", Price = 313.47m, Cost = 194.51m, Stock = 99, ShipAmount = 9, Height = 12, Weight = 13, Width = 7, Length = 8 },
                        new Product { ProductID = "PROD-20250402-018", Description = "Ergonomic and durable.", ProductName = "TP-Link Router", SKU = "KEPY-V758", Price = 566.44m, Cost = 388.77m, Stock = 76, ShipAmount = 6, Height = 10, Weight = 2, Width = 9, Length = 17 },
                        new Product { ProductID = "PROD-20250402-019", Description = "Cutting-edge technology inside.", ProductName = "Sony Xperia", SKU = "PMNX-G799", Price = 694.18m, Cost = 416.96m, Stock = 11, ShipAmount = 6, Height = 5, Weight = 18, Width = 13, Length = 12 },
                        new Product { ProductID = "PROD-20250402-020", Description = "Cutting-edge technology inside.", ProductName = "Dell Monitor", SKU = "WZZF-Z806", Price = 265.24m, Cost = 179.68m, Stock = 92, ShipAmount = 7, Height = 2, Weight = 2, Width = 2, Length = 14 },
                        new Product { ProductID = "PROD-20250402-021", Description = "Slim and lightweight.", ProductName = "Razer Headset", SKU = "UUIJ-Q364", Price = 615.14m, Cost = 407.12m, Stock = 83, ShipAmount = 10, Height = 9, Weight = 17, Width = 15, Length = 19 },
                        new Product { ProductID = "PROD-20250402-022", Description = "Cutting-edge technology inside.", ProductName = "ASUS Laptop", SKU = "OQJM-1282", Price = 1034.56m, Cost = 712.05m, Stock = 68, ShipAmount = 2, Height = 4, Weight = 6, Width = 11, Length = 14 },
                        new Product { ProductID = "PROD-20250402-023", Description = "Top-tier performance.", ProductName = "Google Pixel", SKU = "YWMP-6268", Price = 1272.39m, Cost = 917.62m, Stock = 24, ShipAmount = 9, Height = 4, Weight = 21, Width = 4, Length = 19 },
                        new Product { ProductID = "PROD-20250402-024", Description = "Perfect for home and office.", ProductName = "AMD Ryzen", SKU = "SHXJ-G642", Price = 266.39m, Cost = 168.23m, Stock = 31, ShipAmount = 10, Height = 7, Weight = 17, Width = 12, Length = 4 },
                        new Product { ProductID = "PROD-20250402-025", Description = "Great value for money.", ProductName = "Acer Chromebook", SKU = "DJIU-V164", Price = 1173.8m, Cost = 818.72m, Stock = 36, ShipAmount = 7, Height = 11, Weight = 22, Width = 10, Length = 8 },
                        new Product { ProductID = "PROD-20250402-026", Description = "Built for productivity.", ProductName = "AMD Ryzen", SKU = "BHMJ-N188", Price = 299.73m, Cost = 236.87m, Stock = 63, ShipAmount = 9, Height = 9, Weight = 21, Width = 6, Length = 6 },
                        new Product { ProductID = "PROD-20250402-027", Description = "Cutting-edge technology inside.", ProductName = "Gigabyte GPU", SKU = "SQAN-H031", Price = 41.8m, Cost = 26.31m, Stock = 61, ShipAmount = 3, Height = 8, Weight = 25, Width = 14, Length = 12 },
                        new Product { ProductID = "PROD-20250402-028", Description = "Great value for money.", ProductName = "Apple iPad", SKU = "OSZY-8709", Price = 189.87m, Cost = 133.9m, Stock = 47, ShipAmount = 7, Height = 2, Weight = 3, Width = 8, Length = 6 },
                        new Product { ProductID = "PROD-20250402-029", Description = "Slim and lightweight.", ProductName = "TP-Link Router", SKU = "QLBQ-0466", Price = 101.77m, Cost = 72.39m, Stock = 66, ShipAmount = 7, Height = 7, Weight = 16, Width = 8, Length = 13 },
                        new Product { ProductID = "PROD-20250402-030", Description = "Great value for money.", ProductName = "ASUS Laptop", SKU = "YBBO-C020", Price = 1339.2m, Cost = 807.21m, Stock = 43, ShipAmount = 3, Height = 11, Weight = 22, Width = 15, Length = 17 },
                        new Product { ProductID = "PROD-20250402-031", Description = "Ergonomic and durable.", ProductName = "ASUS Laptop", SKU = "YJFH-I731", Price = 45.68m, Cost = 34.46m, Stock = 56, ShipAmount = 7, Height = 12, Weight = 9, Width = 7, Length = 14 },
                        new Product { ProductID = "PROD-20250402-032", Description = "Great value for money.", ProductName = "Logitech Keyboard", SKU = "BYIM-H692", Price = 599.93m, Cost = 454.61m, Stock = 44, ShipAmount = 10, Height = 3, Weight = 7, Width = 2, Length = 18 },
                        new Product { ProductID = "PROD-20250402-033", Description = "Perfect for home and office.", ProductName = "HP Mouse", SKU = "EFLN-9485", Price = 985.42m, Cost = 629.68m, Stock = 53, ShipAmount = 7, Height = 12, Weight = 25, Width = 12, Length = 10 },
                        new Product { ProductID = "PROD-20250402-034", Description = "Top-tier performance.", ProductName = "TP-Link Router", SKU = "LPEH-Q870", Price = 24.42m, Cost = 16.66m, Stock = 59, ShipAmount = 3, Height = 9, Weight = 9, Width = 10, Length = 16 },
                        new Product { ProductID = "PROD-20250402-035", Description = "Perfect for home and office.", ProductName = "Corsair RAM", SKU = "HKYE-U687", Price = 339.38m, Cost = 208.63m, Stock = 24, ShipAmount = 1, Height = 3, Weight = 21, Width = 8, Length = 13 },
                        new Product { ProductID = "PROD-20250402-036", Description = "Slim and lightweight.", ProductName = "TP-Link Router", SKU = "XJQJ-M448", Price = 366.86m, Cost = 292.13m, Stock = 35, ShipAmount = 2, Height = 7, Weight = 16, Width = 14, Length = 13 },
                        new Product { ProductID = "PROD-20250402-037", Description = "Slim and lightweight.", ProductName = "Samsung SSD", SKU = "DIRQ-T346", Price = 828.19m, Cost = 635.75m, Stock = 67, ShipAmount = 1, Height = 10, Weight = 13, Width = 9, Length = 14 },
                        new Product { ProductID = "PROD-20250402-038", Description = "Built for productivity.", ProductName = "Logitech Keyboard", SKU = "DYYH-Y453", Price = 163.41m, Cost = 124.14m, Stock = 79, ShipAmount = 4, Height = 10, Weight = 11, Width = 15, Length = 5 },
                        new Product { ProductID = "PROD-20250402-039", Description = "Cutting-edge technology inside.", ProductName = "Bose Headphones", SKU = "OPNL-I365", Price = 1478.05m, Cost = 1140.08m, Stock = 43, ShipAmount = 5, Height = 12, Weight = 4, Width = 13, Length = 19 },
                        new Product { ProductID = "PROD-20250402-040", Description = "Top-tier performance.", ProductName = "Apple iPad", SKU = "ULXM-X636", Price = 83.54m, Cost = 55.19m, Stock = 23, ShipAmount = 7, Height = 2, Weight = 17, Width = 10, Length = 11 },
                        new Product { ProductID = "PROD-20250402-041", Description = "Cutting-edge technology inside.", ProductName = "ASUS Laptop", SKU = "TMNI-Y492", Price = 534.41m, Cost = 415.68m, Stock = 20, ShipAmount = 8, Height = 9, Weight = 19, Width = 10, Length = 8 },
                        new Product { ProductID = "PROD-20250402-042", Description = "Cutting-edge technology inside.", ProductName = "Intel CPU", SKU = "GWME-L233", Price = 1345.45m, Cost = 925.91m, Stock = 37, ShipAmount = 10, Height = 2, Weight = 6, Width = 8, Length = 6 },
                        new Product { ProductID = "PROD-20250402-043", Description = "Affordable and reliable.", ProductName = "Corsair RAM", SKU = "PHIA-U404", Price = 1021.0m, Cost = 719.15m, Stock = 26, ShipAmount = 5, Height = 4, Weight = 5, Width = 6, Length = 19 },
                        new Product { ProductID = "PROD-20250402-044", Description = "Affordable and reliable.", ProductName = "Netgear Switch", SKU = "ZLXC-E960", Price = 150.75m, Cost = 93.53m, Stock = 84, ShipAmount = 3, Height = 2, Weight = 22, Width = 11, Length = 20 },
                        new Product { ProductID = "PROD-20250402-045", Description = "Built for productivity.", ProductName = "Sony Xperia", SKU = "EIAY-1207", Price = 610.31m, Cost = 471.74m, Stock = 74, ShipAmount = 7, Height = 5, Weight = 12, Width = 6, Length = 4 },
                        new Product { ProductID = "PROD-20250402-046", Description = "Slim and lightweight.", ProductName = "Apple iPad", SKU = "HELP-T301", Price = 118.01m, Cost = 82.71m, Stock = 36, ShipAmount = 2, Height = 4, Weight = 3, Width = 14, Length = 18 },
                        new Product { ProductID = "PROD-20250402-047", Description = "Built for productivity.", ProductName = "Gigabyte GPU", SKU = "ZPZR-X946", Price = 1183.06m, Cost = 880.03m, Stock = 100, ShipAmount = 6, Height = 5, Weight = 13, Width = 10, Length = 14 },
                        new Product { ProductID = "PROD-20250402-048", Description = "Crystal-clear display.", ProductName = "ASUS Laptop", SKU = "THZW-C785", Price = 1140.24m, Cost = 719.7m, Stock = 72, ShipAmount = 2, Height = 3, Weight = 14, Width = 15, Length = 5 },
                        new Product { ProductID = "PROD-20250402-049", Description = "Affordable and reliable.", ProductName = "Logitech Keyboard", SKU = "TABZ-F692", Price = 1009.11m, Cost = 614.24m, Stock = 26, ShipAmount = 3, Height = 5, Weight = 16, Width = 5, Length = 20 },
                        new Product { ProductID = "PROD-20250402-050", Description = "Perfect for home and office.", ProductName = "AMD Ryzen", SKU = "DZYI-2757", Price = 354.34m, Cost = 217.62m, Stock = 36, ShipAmount = 1, Height = 3, Weight = 13, Width = 5, Length = 11 }
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