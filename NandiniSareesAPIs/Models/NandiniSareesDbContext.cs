using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NandiniSareesAPIs.Models
{
    // CQRS-friendly context interfaces
    public interface IReadDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Role> Roles { get; }
        DbSet<UserRole> UserRoles { get; }
        DbSet<Category> Categories { get; }
        DbSet<Product> Products { get; }
        DbSet<ProductImage> ProductImages { get; }
        DbSet<Tag> Tags { get; }
        DbSet<ProductTag> ProductTags { get; }
        DbSet<Order> Orders { get; }
        DbSet<OrderItem> OrderItems { get; }
        DbSet<ShippingAddress> ShippingAddresses { get; }
        DbSet<Payment> Payments { get; }
        DbSet<Review> Reviews { get; }
        DbSet<Cart> Carts { get; }
        DbSet<CartItem> CartItems { get; }
    }

    public interface IWriteDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }

    public class NandiniSareesDbContext : DbContext, IReadDbContext, IWriteDbContext
    {
        public NandiniSareesDbContext(DbContextOptions<NandiniSareesDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Users
            modelBuilder.Entity<User>(b =>
            {
                b.ToTable("Users");
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.Email).IsUnique();
                b.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
                b.Property(x => x.LastName).HasMaxLength(100);
                b.Property(x => x.Email).IsRequired().HasMaxLength(256);
                b.Property(x => x.PasswordHash).HasMaxLength(512);
                b.Property(x => x.IsActive).HasDefaultValue(true);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            });

            // Roles
            modelBuilder.Entity<Role>(b =>
            {
                b.ToTable("Roles");
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.Name).IsUnique();
                b.Property(x => x.Name).IsRequired().HasMaxLength(100);
            });

            // UserRoles (many-to-many)
            modelBuilder.Entity<UserRole>(b =>
            {
                b.ToTable("UserRoles");
                b.HasKey(x => new { x.UserId, x.RoleId });
                b.HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.Role).WithMany(x => x.UserRoles).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);
            });

            // Categories
            modelBuilder.Entity<Category>(b =>
            {
                b.ToTable("Categories");
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired().HasMaxLength(200);
                b.HasIndex(x => x.ParentCategoryId);
                b.HasOne(x => x.Parent).WithMany(x => x.Children).HasForeignKey(x => x.ParentCategoryId).OnDelete(DeleteBehavior.Restrict);
            });

            // Products
            modelBuilder.Entity<Product>(b =>
            {
                b.ToTable("Products");
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired().HasMaxLength(300);
                b.Property(x => x.SKU).HasMaxLength(100);
                b.HasIndex(x => x.CategoryId);
                // Unique SKU when not null
                b.HasIndex(x => x.SKU).IsUnique().HasFilter("[SKU] IS NOT NULL");
                b.Property(x => x.Price).HasColumnType("decimal(18,2)").HasDefaultValue(0.00m);
                b.Property(x => x.Stock).HasDefaultValue(0);
                b.Property(x => x.IsActive).HasDefaultValue(true);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                b.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.SetNull);
            });

            // ProductImages
            modelBuilder.Entity<ProductImage>(b =>
            {
                b.ToTable("ProductImages");
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.ProductId);
                b.Property(x => x.Url).IsRequired().HasMaxLength(1000);
                b.HasOne(x => x.Product).WithMany(x => x.Images).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Cascade);
            });

            // Tags
            modelBuilder.Entity<Tag>(b =>
            {
                b.ToTable("Tags");
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.Name).IsUnique();
                b.Property(x => x.Name).IsRequired().HasMaxLength(200);
            });

            // ProductTags (many-to-many)
            modelBuilder.Entity<ProductTag>(b =>
            {
                b.ToTable("ProductTags");
                b.HasKey(x => new { x.ProductId, x.TagId });
                b.HasOne(x => x.Product).WithMany(x => x.ProductTags).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.Tag).WithMany(x => x.ProductTags).HasForeignKey(x => x.TagId).OnDelete(DeleteBehavior.Cascade);
            });

            // Orders
            modelBuilder.Entity<Order>(b =>
            {
                b.ToTable("Orders");
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.OrderNumber).IsUnique();
                b.HasIndex(x => x.UserId);
                b.Property(x => x.OrderDate).HasDefaultValueSql("SYSUTCDATETIME()");
                b.Property(x => x.Subtotal).HasColumnType("decimal(18,2)");
                b.HasOne(x => x.User).WithMany(x => x.Orders).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.SetNull);
                b.HasOne(x => x.ShippingAddress).WithMany().HasForeignKey(x => x.ShippingAddressId).OnDelete(DeleteBehavior.SetNull);
                b.HasOne(x => x.Payment).WithOne().HasForeignKey<Order>(o => o.PaymentId).OnDelete(DeleteBehavior.SetNull);
            });

            // OrderItems
            modelBuilder.Entity<OrderItem>(b =>
            {
                b.ToTable("OrderItems");
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.OrderId);
                b.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
                // Computed column mapping
                b.Property(x => x.TotalPrice).HasComputedColumnSql("([Quantity] * [UnitPrice])", stored: true);
                b.HasOne(x => x.Order).WithMany(x => x.Items).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.NoAction);
            });

            // ShippingAddresses
            modelBuilder.Entity<ShippingAddress>(b =>
            {
                b.ToTable("ShippingAddresses");
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.UserId);
                b.HasOne(x => x.User).WithMany(x => x.ShippingAddresses).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.SetNull);
            });

            // Payments
            modelBuilder.Entity<Payment>(b =>
            {
                b.ToTable("Payments");
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.OrderId);
                b.Property(x => x.Amount).HasColumnType("decimal(18,2)");
                b.HasOne(x => x.Order).WithOne().HasForeignKey<Payment>(p => p.OrderId).OnDelete(DeleteBehavior.SetNull);
                b.HasOne(x => x.User).WithMany(x => x.Payments).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.SetNull);
            });

            // Reviews
            modelBuilder.Entity<Review>(b =>
            {
                b.ToTable("Reviews");
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.ProductId);
                b.Property(x => x.Rating).IsRequired();
                b.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                b.HasOne(x => x.Product).WithMany(x => x.Reviews).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.User).WithMany(x => x.Reviews).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.SetNull);
            });

            // Carts
            modelBuilder.Entity<Cart>(b =>
            {
                b.ToTable("Carts");
                b.HasKey(x => x.Id);
                b.HasOne(x => x.User).WithMany(x => x.Carts).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.SetNull);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            });

            // CartItems
            modelBuilder.Entity<CartItem>(b =>
            {
                b.ToTable("CartItems");
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.CartId);
                b.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
                b.HasOne(x => x.Cart).WithMany(x => x.Items).HasForeignKey(x => x.CartId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.NoAction);
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }

    // Entity classes moved to separate files under Models/Entities for clarity and maintainability.
}
