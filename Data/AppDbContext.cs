
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Models;
using System.Collections;

namespace OrderManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets for all models
        public DbSet<Billing> Billing { get; set; }
        public DbSet<BillingAccount> BillingAccount { get; set; }
        public DbSet<Charge> Charge { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UsersRole> UserRole { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<FreightOutbound> FreightOutbound { get; set; }
        public DbSet<FreightProductList> FreightProductList { get; set; }
        public DbSet<InboundOrder> InboundOrder { get; set; }
        public DbSet<InboundProductList> InboundProductList { get; set; }
        public DbSet<ParcelProductList> ParcelProductList { get; set; }
        public DbSet<PlatformOrder> PlatformOrder { get; set; }
        public DbSet<PlatformProductList> PlatformProductList { get; set; }
        public DbSet<ParcelOutbound> ParcelOutbound { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
        public DbSet<Customer> Customers { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Billing configuration
            modelBuilder.Entity<Billing>(entity =>
            {
                entity.HasKey(b => b.BillingAccountId);
                entity.Property(b => b.BillingAccountId)
                      .HasColumnName("Billing_ID")
                      .HasMaxLength(25);

                entity.Property(b => b.BillingAccountId)
                      .HasColumnName("Billing_Account_ID")
                      .HasMaxLength(25)
                      .IsRequired();

                entity.Property(b => b.ChargeId)
                      .HasColumnName("Charge_ID")
                      .HasMaxLength(25)
                      .IsRequired();

                entity.Property(b => b.Amount)
                      .HasColumnName("Amount")
                      .HasColumnType("decimal(18, 2)");

                entity.Property(b => b.DateCreated)
                      .HasColumnName("Date_Created");

                // Relationships
                entity.HasOne(b => b.BillingAccount)
                      .WithMany(ba => ba.Billing)
                      .HasForeignKey(b => b.BillingAccountId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(b => b.Charge)
                      .WithMany(c => c.Billing)
                      .HasForeignKey(b => b.ChargeId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // BillingAccount configuration
            modelBuilder.Entity<BillingAccount>(entity =>
            {
                entity.HasKey(ba => ba.BillingAccountId);
                entity.Property(ba => ba.BillingAccountId)
                      .HasColumnName("Billing_Account_ID")
                      .HasMaxLength(25);

                entity.Property(ba => ba.UserId)
                      .HasColumnName("User_ID")
                      .HasMaxLength(25)
                      .IsRequired();

                entity.Property(ba => ba.AccountBalance)
                      .HasColumnName("Account_Balance")
                      .HasColumnType("decimal(10, 2)");

                // Relationships
                entity.HasOne(ba => ba.User)
                      .WithMany(u => u.BillingAccount)
                      .HasForeignKey(ba => ba.UserId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // Charge configuration
            modelBuilder.Entity<Charge>(entity =>
            {
                entity.HasKey(c => c.ChargeId);
                entity.Property(c => c.ChargeId)
                      .HasColumnName("Charge_ID")
                      .HasMaxLength(25);

                entity.Property(c => c.Amount)
                      .HasColumnName("Amount")
                      .HasColumnType("decimal(18, 2)")
                      .IsRequired();

                entity.Property(c => c.ChargeType)
                      .HasColumnName("Charge_Type")
                      .HasMaxLength(25);

                entity.Property(c => c.Description)
                      .HasColumnName("Description")
                      .HasMaxLength(255);
            });

            // Order configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.OrderId);

                entity.Property(o => o.OrderId)
                      .HasColumnName("Order_ID")
                      .HasMaxLength(25);

                entity.Property(o => o.UserId)
                      .HasColumnName("User_ID")
                      .HasMaxLength(25)
                      .IsRequired();

                entity.Property(o => o.TotalAmount)
                      .HasColumnName("TotalAmount")
                      .HasColumnType("decimal(18, 2)");

                entity.Property(o => o.OrderDate)
                      .HasColumnName("OrderDate");

                // Relationships
                entity.HasOne(o => o.User)
                      .WithMany(u => u.Order)
                      .HasForeignKey(o => o.UserId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // OrderItem configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => new { oi.OrderId, oi.ProductId });

                entity.Property(oi => oi.OrderId)
                      .HasColumnName("Order_ID")
                      .HasMaxLength(25);

                entity.Property(oi => oi.ProductId)
                      .HasColumnName("Product_ID")
                      .HasMaxLength(25);

                entity.Property(oi => oi.Quantity)
                      .HasColumnName("Quantity")
                      .IsRequired();

                entity.Property(oi => oi.UnitPrice)
                      .HasColumnName("UnitPrice")
                      .HasColumnType("decimal(18, 2)")
                      .IsRequired();

                // Relationships
                entity.HasOne(oi => oi.Order)
                      .WithMany(o => o.OrderItem)
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(oi => oi.Inventory)
                      .WithMany(i => i.OrderItem)
                      .HasForeignKey(oi => oi.ProductId)
                      .OnDelete(DeleteBehavior.NoAction);
            });


           

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);

                entity.Property(u => u.UserId)
                      .HasColumnName("User_ID")
                      .HasMaxLength(25);

                entity.Property(u => u.Username)
                      .HasColumnName("Username")
                      .HasMaxLength(25)
                      .IsRequired();

                entity.Property(u => u.Password)
                      .HasColumnName("Password")
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(u => u.Email)
                      .HasColumnName("Email")
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(u => u.DateCreated)
                      .HasColumnName("Date_Created");

                // Relationships
                entity.HasMany(u => u.BillingAccount)
                      .WithOne(ba => ba.User)
                      .HasForeignKey(ba => ba.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(u => u.UserRole)
                      .WithOne(ur => ur.Users)
                      .HasForeignKey(ur => ur.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(u => u.Order)
                      .WithOne(o => o.User)
                      .HasForeignKey(o => o.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(u => u.FreightOutbound)
                      .WithOne(fo => fo.User)
                      .HasForeignKey(fo => fo.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(u => u.InboundOrder)
                      .WithOne(io => io.User)
                      .HasForeignKey(io => io.User_ID)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(u => u.ParcelOutbound)
                      .WithOne(po => po.User)
                      .HasForeignKey(po => po.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(u => u.PlatformOrder)
                      .WithOne(po => po.User)
                      .HasForeignKey(po => po.UserId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // Role configuration
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.RoleId);

                entity.Property(r => r.RoleId)
                      .HasColumnName("Role_ID")
                      .HasMaxLength(25);

                entity.Property(r => r.RoleName)
                      .HasColumnName("RoleName")
                      .HasMaxLength(25)
                      .IsRequired();

                entity.Property(r => r.RoleDescription)
                      .HasColumnName("Role_Description")
                      .HasMaxLength(255);

                // Relationships
                entity.HasMany(r => r.UserRole)
                      .WithOne(ur => ur.Role)
                      .HasForeignKey(ur => ur.RoleId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // UserRole configuration
            modelBuilder.Entity<UsersRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.Property(ur => ur.UserId)
                      .HasColumnName("User_ID")
                      .HasMaxLength(25);

                entity.Property(ur => ur.RoleId)
                      .HasColumnName("Role_ID")
                      .HasMaxLength(25);

                // Relationships are configured in User and Role entities
            });

            // Inventory configuration
            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(i => i.Product_ID);

                entity.Property(i => i.Product_ID)
                      .HasColumnName("Product_ID")
                      .HasMaxLength(25);

                entity.Property(i => i.Warehouse_ID)
                      .HasColumnName("Warehouse_ID")
                      .HasMaxLength(25)
                      .IsRequired();

                entity.Property(i => i.SKU)
                      .HasColumnName("SKU")
                      .HasMaxLength(50);

                entity.Property(i => i.Product_Name)
                      .HasColumnName("Product_Name")
                      .HasMaxLength(255);

                entity.Property(i => i.Product_Description)
                      .HasColumnName("Product_Description")
                      .HasMaxLength(255);

                entity.Property(i => i.Price)
                      .HasColumnName("Price")
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();

                entity.Property(i => i.Quantity)
                      .HasColumnName("Quantity")
                      .IsRequired();

                // Relationships
                entity.HasOne(i => i.Warehouse)
                      .WithMany(w => w.Inventory)
                      .HasForeignKey(i => i.Warehouse_ID)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // FreightOutbound configuration
            modelBuilder.Entity<FreightOutbound>(entity =>
            {
                entity.HasKey(fo => fo.OutboundOrderId);

                entity.Property(fo => fo.OutboundOrderId)
                      .HasColumnName("Outbound_Order_ID")
                      .HasMaxLength(25);

                entity.Property(fo => fo.OrderType)
                      .HasColumnName("Order_Type")
                      .HasMaxLength(25);

                entity.Property(fo => fo.OrderStatus)
                      .HasColumnName("Order_Status")
                      .HasMaxLength(25);

                entity.Property(fo => fo.UserId)
                      .HasColumnName("User_ID")
                      .HasMaxLength(25)
                      .IsRequired();

                entity.Property(fo => fo.Warehouse_ID)
                      .HasColumnName("Warehouse_ID")
                      .HasMaxLength(25)
                      .IsRequired();

                entity.Property(fo => fo.ProductQuantity)
                      .HasColumnName("Product_Quantity");

                entity.Property(fo => fo.CreationDate)
                      .HasColumnName("Creation_Date");

                entity.Property(fo => fo.EstimatedDeliveryDate)
                      .HasColumnName("Estimated_Delivery_Date");

                entity.Property(fo => fo.OrderShipDate)
                      .HasColumnName("Order_Ship_Date");

                entity.Property(fo => fo.Cost)
                      .HasColumnName("Cost")
                      .HasColumnType("decimal(10, 2)");

                entity.Property(fo => fo.Currency)
                      .HasColumnName("Currency")
                      .HasMaxLength(25);

                entity.Property(fo => fo.Recipient)
                      .HasColumnName("Recipient")
                      .HasMaxLength(100);

                entity.Property(fo => fo.RecipientPostCode)
                      .HasColumnName("Recipient_Postcode")
                      .HasMaxLength(50);

                entity.Property(fo => fo.DestinationType)
                      .HasColumnName("Destination_Type")
                      .HasMaxLength(50);

                entity.Property(fo => fo.Platform)
                      .HasColumnName("Platform")
                      .HasMaxLength(50);

                entity.Property(fo => fo.ShippingCompany)
                      .HasColumnName("Shipping_Company")
                      .HasMaxLength(50);

                entity.Property(fo => fo.TransportDays)
                      .HasColumnName("Transport_Days");

                entity.Property(fo => fo.RelatedAdjustmentOrder)
                      .HasColumnName("Related_Adjustment_Order")
                      .HasMaxLength(25);

                entity.Property(fo => fo.TrackingNumber)
                      .HasColumnName("Tracking_Number")
                      .HasMaxLength(255);

                entity.Property(fo => fo.ReferenceOrderNumber)
                      .HasColumnName("Reference_Order_Number")
                      .HasMaxLength(255);

                entity.Property(fo => fo.FBAShipmentId)
                      .HasColumnName("FBA_Shipment_ID")
                      .HasMaxLength(25);

                entity.Property(fo => fo.FBATrackingNumber)
                      .HasColumnName("FBA_Tracking_Number")
                      .HasMaxLength(25);

                entity.Property(fo => fo.OutboundMethod)
                      .HasColumnName("Outbound_Method")
                      .HasMaxLength(25);

                // Relationships
                entity.HasOne(fo => fo.User)
                      .WithMany(u => u.FreightOutbound)
                      .HasForeignKey(fo => fo.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(fo => fo.Warehouse)
                      .WithMany(w => w.FreightOutbound)
                      .HasForeignKey(fo => fo.Warehouse_ID)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // FreightProductList configuration
            modelBuilder.Entity<FreightProductList>(entity =>
            {
                entity.HasKey(fpl => new { fpl.OrderId, fpl.ProductId });

                entity.Property(fpl => fpl.OrderId)
                      .HasColumnName("Order_ID")
                      .HasMaxLength(25);

                entity.Property(fpl => fpl.ProductId)
                      .HasColumnName("Product_ID")
                      .HasMaxLength(25);

                entity.Property(fpl => fpl.Quantity)
                      .HasColumnName("Quantity")
                      .IsRequired();

                // Relationships
                entity.HasOne(fpl => fpl.FreightOutbound)
                      .WithMany(fo => fo.FreightProductList)
                      .HasForeignKey(fpl => fpl.OrderId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(fpl => fpl.Inventory)
                      .WithMany(i => i.FreightProductList)
                      .HasForeignKey(fpl => fpl.ProductId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // InboundOrder configuration
            modelBuilder.Entity<InboundOrder>(entity =>
            {
                entity.HasKey(io => io.InboundOrderId);

                entity.Property(io => io.InboundOrderId)
                      .HasColumnName("Inbound_Order_ID")
                      .HasMaxLength(25);

                entity.Property(io => io.OrderStatus)
                      .HasColumnName("Order_Status")
                      .HasMaxLength(25);

                entity.Property(io => io.User_ID)
                      .HasColumnName("User_ID")
                      .HasMaxLength(25)
                      .IsRequired();

                entity.Property(io => io.Warehouse_ID)
                      .HasColumnName("Warehouse_ID")
                      .HasMaxLength(25)
                      .IsRequired();

                entity.Property(io => io.EstimatedArrival)
                      .HasColumnName("Estimated_Arrival");

                entity.Property(io => io.ProductQuantity)
                      .HasColumnName("Product_Quantity");

                entity.Property(io => io.CreationDate)
                      .HasColumnName("Creation_Date");

                entity.Property(io => io.Cost)
                      .HasColumnName("Cost")
                      .HasColumnType("decimal(10, 2)");

                entity.Property(io => io.Currency)
                      .HasColumnName("Currency")
                      .HasMaxLength(50);

                entity.Property(io => io.Boxes)
                      .HasColumnName("Boxes");

                entity.Property(io => io.InboundType)
                      .HasColumnName("Inbound_Type")
                      .HasMaxLength(25);

                entity.Property(io => io.TrackingNumber)
                      .HasColumnName("Tracking_Number")
                      .HasMaxLength(255);

                entity.Property(io => io.ReferenceOrderNumber)
                      .HasColumnName("Reference_Order_Number")
                      .HasMaxLength(255);

                entity.Property(io => io.ArrivalMethod)
                      .HasColumnName("Arrival_Method")
                      .HasMaxLength(25);

                // Relationships
                entity.HasOne(io => io.User)
                      .WithMany(u => u.InboundOrder)
                      .HasForeignKey(io => io.User_ID)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(io => io.Warehouse)
                      .WithMany(w => w.InboundOrder)
                      .HasForeignKey(io => io.Warehouse_ID)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // InboundProductList configuration
            modelBuilder.Entity<InboundProductList>(entity =>
            {
                entity.HasKey(ipl => new { ipl.OrderId, ipl.ProductId });

                entity.Property(ipl => ipl.OrderId)
                      .HasColumnName("Inbound_Order_ID")
                      .HasMaxLength(25);

                entity.Property(ipl => ipl.ProductId)
                      .HasColumnName("Product_ID")
                      .HasMaxLength(25);

                entity.Property(ipl => ipl.Quantity)
                      .HasColumnName("Quantity")
                      .IsRequired();

                // Relationships
                entity.HasOne(ipl => ipl.InboundOrder)
                      .WithMany(io => io.InboundProductList)
                      .HasForeignKey(ipl => ipl.OrderId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(ipl => ipl.Inventory)
                      .WithMany(i => i.InboundProductList)
                      .HasForeignKey(ipl => ipl.ProductId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // ParcelOutbound configuration
            modelBuilder.Entity<ParcelOutbound>(entity =>
            {
                entity.HasKey(po => po.OrderId);

                entity.Property(po => po.OrderId)
                      .HasColumnName("Order_ID")
                      .HasMaxLength(25);

                entity.Property(po => po.OrderType)
                      .HasColumnName("Order_Type")
                      .HasMaxLength(25);

                entity.Property(po => po.OrderStatus)
                      .HasColumnName("Order_Status")
                      .HasMaxLength(25);

                entity.Property(po => po.Warehouse_ID)
                      .HasColumnName("Warehouse_ID")
                      .HasMaxLength(25)
                      .IsRequired();

                entity.Property(po => po.UserId)
                      .HasColumnName("User_ID")
                      .HasMaxLength(25)
                      .IsRequired();

                entity.Property(po => po.Platform)
                      .HasColumnName("Platform")
                      .HasMaxLength(50);

                entity.Property(po => po.EstimatedDeliveryDate)
                      .HasColumnName("Estimated_Delivery_Date");

                entity.Property(po => po.ShipDate)
                      .HasColumnName("Ship_Date");

                entity.Property(po => po.TransportDays)
                      .HasColumnName("Transport_Days");

                entity.Property(po => po.Cost)
                      .HasColumnName("Cost")
                      .HasColumnType("decimal(10, 2)");

                entity.Property(po => po.Currency)
                      .HasColumnName("Currency")
                      .HasMaxLength(25);

                entity.Property(po => po.Recipient)
                      .HasColumnName("Recipient")
                      .HasMaxLength(50);

                entity.Property(po => po.Country)
                      .HasColumnName("Country")
                      .HasMaxLength(50);

                entity.Property(po => po.Postcode)
                      .HasColumnName("Postcode")
                      .HasMaxLength(25);

                entity.Property(po => po.TrackingNumber)
                      .HasColumnName("Tracking_Number")
                      .HasMaxLength(25);

                entity.Property(po => po.ReferenceOrderNumber)
                      .HasColumnName("Reference_Order_Number")
                      .HasMaxLength(25);

                entity.Property(po => po.CreationDate)
                      .HasColumnName("Creation_Date");

                entity.Property(po => po.Boxes)
                      .HasColumnName("Boxes");

                entity.Property(po => po.ShippingCompany)
                      .HasColumnName("Shipping_Company")
                      .HasMaxLength(50);

                entity.Property(po => po.LatestInformation)
                      .HasColumnName("Latest_Information")
                      .HasMaxLength(255);

                entity.Property(po => po.TrackingUpdateTime)
                      .HasColumnName("Tracking_Update_Time");

                entity.Property(po => po.InternetPostingTime)
                      .HasColumnName("Internet_Posting_Time");

                entity.Property(po => po.DeliveryTime)
                      .HasColumnName("Delivery_Time");

                entity.Property(po => po.RelatedAdjustmentOrder)
                      .HasColumnName("Related_Adjustment_Order")
                      .HasMaxLength(25);

                // Relationships
                entity.HasOne(po => po.User)
                      .WithMany(u => u.ParcelOutbound)
                      .HasForeignKey(po => po.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(po => po.Warehouse)
                      .WithMany(w => w.ParcelOutbound)
                      .HasForeignKey(po => po.Warehouse_ID)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // ParcelProductList configuration
            modelBuilder.Entity<ParcelProductList>(entity =>
            {
                entity.HasKey(ppl => new { ppl.OrderId, ppl.ProductId });

                entity.Property(ppl => ppl.OrderId)
                      .HasColumnName("Order_ID")
                      .HasMaxLength(25);

                entity.Property(ppl => ppl.ProductId)
                      .HasColumnName("Product_ID")
                      .HasMaxLength(25);

                entity.Property(ppl => ppl.Quantity)
                      .HasColumnName("Quantity")
                      .IsRequired();

                // Relationships
                entity.HasOne(ppl => ppl.ParcelOutbound)
                      .WithMany(po => po.ParcelProductList)
                      .HasForeignKey(ppl => ppl.OrderId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(ppl => ppl.Inventory)
                      .WithMany(i => i.ParcelProductList)
                      .HasForeignKey(ppl => ppl.ProductId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // PlatformOrder configuration
            modelBuilder.Entity<PlatformOrder>(entity =>
            {
                entity.HasKey(po => po.OrderId);

                entity.Property(po => po.OrderId)
                      .HasColumnName("Order_ID")
                      .HasMaxLength(25);

                entity.Property(po => po.Platform)
                      .HasColumnName("Platform")
                      .HasMaxLength(25);

                entity.Property(po => po.Warehouse_ID)
                      .HasColumnName("Warehouse_ID")
                      .HasMaxLength(25)
                      .IsRequired();

                entity.Property(po => po.ProductQuantity)
                      .HasColumnName("Product_Quantity");

                entity.Property(po => po.UserId)
                      .HasColumnName("User_ID")
                      .HasMaxLength(25)
                      .IsRequired();

                entity.Property(po => po.Buyer)
                      .HasColumnName("Buyer")
                      .HasMaxLength(25);

                entity.Property(po => po.RecipientPostcode)
                      .HasColumnName("Recipient_Postcode")
                      .HasMaxLength(25);

                entity.Property(po => po.RecipientCountry)
                      .HasColumnName("Recipient_Country")
                      .HasMaxLength(25);

                entity.Property(po => po.Store)
                      .HasColumnName("Store")
                      .HasMaxLength(25);

                entity.Property(po => po.Site)
                      .HasColumnName("Site")
                      .HasMaxLength(25);

                entity.Property(po => po.ShippingService)
                      .HasColumnName("Shipping_Service")
                      .HasMaxLength(25);

                entity.Property(po => po.TrackingNumber)
                      .HasColumnName("Tracking_Number")
                      .HasMaxLength(255);

                entity.Property(po => po.Carrier)
                      .HasColumnName("Carrier")
                      .HasMaxLength(25);

                entity.Property(po => po.OrderTime)
                      .HasColumnName("Order_Time");

                entity.Property(po => po.PaymentTime)
                      .HasColumnName("Payment_Time");

                entity.Property(po => po.CreatedTime)
                      .HasColumnName("Created_Time");

                entity.Property(po => po.OrderSource)
                      .HasColumnName("Order_Source")
                      .HasMaxLength(25);

                // Relationships
                entity.HasOne(po => po.User)
                      .WithMany(u => u.PlatformOrder)
                      .HasForeignKey(po => po.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(po => po.Warehouse)
                      .WithMany(w => w.PlatformOrder)
                      .HasForeignKey(po => po.Warehouse_ID)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // PlatformProductList configuration
            modelBuilder.Entity<PlatformProductList>(entity =>
            {
                entity.HasKey(pp => new { pp.OrderId, pp.ProductId });

                entity.Property(pp => pp.OrderId)
                      .HasColumnName("Order_ID")
                      .HasMaxLength(25);

                entity.Property(pp => pp.ProductId)
                      .HasColumnName("Product_ID")
                      .HasMaxLength(25);

                entity.Property(pp => pp.Quantity)
                      .HasColumnName("Quantity")
                      .IsRequired();

                // Relationships
                entity.HasOne(pp => pp.PlatformOrder)
                      .WithMany(po => po.PlatformProductList)
                      .HasForeignKey(pp => pp.OrderId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(pp => pp.Inventory)
                      .WithMany(i => i.PlatformProductList)
                      .HasForeignKey(pp => pp.ProductId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // Warehouse configuration
            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.HasKey(w => w.Warehouse_ID);

                entity.Property(w => w.Warehouse_ID)
                      .HasColumnName("Warehouse_ID")
                      .HasMaxLength(25);

                entity.Property(w => w.Name)
                      .HasColumnName("Warehouse")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(w => w.Country)
                      .HasColumnName("Country")
                      .HasMaxLength(50);

                entity.Property(w => w.City)
                      .HasColumnName("City")
                      .HasMaxLength(50);

                entity.Property(w => w.Currency)
                      .HasColumnName("Currency")
                      .HasMaxLength(50);

                // Relationships
                entity.HasMany(w => w.Inventory)
                      .WithOne(i => i.Warehouse)
                      .HasForeignKey(i => i.Warehouse_ID)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(w => w.InboundOrder)
                      .WithOne(io => io.Warehouse)
                      .HasForeignKey(io => io.Warehouse_ID)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(w => w.FreightOutbound)
                      .WithOne(fo => fo.Warehouse)
                      .HasForeignKey(fo => fo.Warehouse_ID)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(w => w.ParcelOutbound)
                      .WithOne(po => po.Warehouse)
                      .HasForeignKey(po => po.Warehouse_ID)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(w => w.PlatformOrder)
                      .WithOne(po => po.Warehouse)
                      .HasForeignKey(po => po.Warehouse_ID)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // Customer configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.UserId);

                entity.Property(c => c.UserId)
                      .HasColumnName("User_ID")
                      .HasMaxLength(25);

                entity.Property(c => c.AdminId)
                      .HasColumnName("Admin_ID")
                      .HasMaxLength(25);

                entity.Property(c => c.CompanyName)
                      .HasColumnName("Company_Name")
                      .HasMaxLength(50);

                entity.Property(c => c.AccountStatus)
                      .HasColumnName("Account_Status")
                      .HasMaxLength(25);

                entity.Property(c => c.ProductNeedAuditFree)
                      .HasColumnName("Product_Need_Audit_Free")
                      .HasMaxLength(25);

                entity.Property(c => c.WarehouseAvailability)
                      .HasColumnName("Warehouse_Availability");

                entity.Property(c => c.BillingAccountId)
                      .HasColumnName("Billing_Account_ID")
                      .HasMaxLength(25);

                entity.Property(c => c.DateCreated)
                      .HasColumnName("Date_Created");

                // Relationships
                entity.HasOne(c => c.Administrator)
                      .WithMany()
                      .HasForeignKey(c => c.AdminId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(c => c.CustomerUser)
                      .WithMany()
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(c => c.BillingAccount)
                      .WithMany()
                      .HasForeignKey(c => c.BillingAccountId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            

            base.OnModelCreating(modelBuilder);
        }
    }
}
