using ECommerceAPI.Model;
using EcommerceAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TestItem>().HasData(
                new TestItem { Id = 1, Name = "Test Item 1" },
                new TestItem { Id = 2, Name = "Test Item 2" },
                new TestItem { Id = 3, Name = "Test Item 3" }
            );
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Product 1", Price = 10.99m, Category = "Category 1", ImageUrl = "https://example.com/product1.jpg", Stock = 100 },
                new Product { Id = 2, Name = "Product 2", Price = 15.99m, Category = "Category 2", ImageUrl = "https://example.com/product2.jpg", Stock = 50 },
                new Product { Id = 3, Name = "Product 3", Price = 20.99m, Category = "Category 3", ImageUrl = "https://example.com/product3.jpg", Stock = 25 }
            );

            // User -> Orders
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            // Order -> OrderItems
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne()
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product -> OrderItems
            modelBuilder.Entity<OrderItem>()
                .HasOne<Product>()
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2);
        }

        public DbSet<TestItem> TestItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

    }
}