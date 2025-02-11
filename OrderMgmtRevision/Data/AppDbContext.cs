using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using OrderMgmtRevision.Models;

namespace OrderMgmtRevision.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets for Models
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Inventory> InventoryAll { get; set; }
        public DbSet<Shipment> Shipments { get; set; }

        // Configure model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specify precision and scale for the decimal fields, 18 digits so we can have 1234567890123456.99.
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.Cost)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Shipment>()
                .Property(s => s.Cost)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Shipment>()
                .HasOne(s => s.Product)
                .WithMany()
                .HasForeignKey(s => s.ProductID)
                .OnDelete(DeleteBehavior.NoAction); // No cascading delete on source warehouse

            // Configuring the delete behaviors
            modelBuilder.Entity<Shipment>()
                .HasOne(s => s.SourceWarehouse)
                .WithMany()
                .HasForeignKey(s => s.SourceWarehouseID)
                .OnDelete(DeleteBehavior.NoAction); // No cascading delete on source warehouse

            modelBuilder.Entity<Shipment>()
                .HasOne(s => s.DestinationWarehouse)
                .WithMany()
                .HasForeignKey(s => s.DestinationWarehouseID)
                .OnDelete(DeleteBehavior.NoAction); // Restricted delete, so throw an error if there is no shipment attached.



            base.OnModelCreating(modelBuilder);
        }

    }
}
