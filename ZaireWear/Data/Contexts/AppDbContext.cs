using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductCategories> ProductCategories { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<ProductColors> ProductColors { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<ProductSizes> ProductSizes { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<BasketProduct> BasketProducts { get; set; }
    public DbSet<FavoriteProduct> FavoriteProducts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<ContactMessage> ContactMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProductCategories>()
            .HasKey(pc => new { pc.ProductId, pc.CategoryId });

        modelBuilder.Entity<ProductCategories>()
            .HasOne(pc => pc.Product)
            .WithMany(p => p.ProductCategories)
            .HasForeignKey(pc => pc.ProductId);

        modelBuilder.Entity<ProductCategories>()
            .HasOne(pc => pc.Category)
            .WithMany(c => c.ProductCategories)
            .HasForeignKey(pc => pc.CategoryId);

        modelBuilder.Entity<ProductColors>()
        .HasKey(pc => new { pc.ProductId, pc.ColorId });

        modelBuilder.Entity<ProductColors>()
            .HasOne(pc => pc.Product)
            .WithMany(p => p.ProductColors)
            .HasForeignKey(pc => pc.ProductId);

        modelBuilder.Entity<ProductColors>()
            .HasOne(pc => pc.Color)
            .WithMany(c => c.ProductColors)
            .HasForeignKey(pc => pc.ColorId);

        modelBuilder.Entity<ProductSizes>()
        .HasKey(ps => new { ps.ProductId, ps.SizeId });

        modelBuilder.Entity<ProductSizes>()
            .HasOne(ps => ps.Product)
            .WithMany(p => p.ProductSizes)
            .HasForeignKey(ps => ps.ProductId);

        modelBuilder.Entity<ProductSizes>()
            .HasOne(ps => ps.Size)
            .WithMany(s => s.ProductSizes)
            .HasForeignKey(ps => ps.SizeId);

        modelBuilder.Entity<BasketProduct>()
            .HasKey(bp => new { bp.BasketId, bp.ProductId, bp.ColorId, bp.SizeId});

        modelBuilder.Entity<BasketProduct>()
            .HasOne(bp => bp.Basket)
            .WithMany(b => b.BasketProducts)
            .HasForeignKey(bp => bp.BasketId);

        modelBuilder.Entity<Basket>(b =>
        {
            b.HasOne(b => b.User)
             .WithOne(u => u.Basket)
             .HasForeignKey<Basket>(b => b.UserId);

            b.HasMany(b => b.BasketProducts)
             .WithOne(bp => bp.Basket)
             .HasForeignKey(bp => bp.BasketId);
        });

        modelBuilder.Entity<BasketProduct>(bp =>
        {
            bp.HasOne(bp => bp.Product)
              .WithMany(p => p.BasketProducts)
              .HasForeignKey(bp => bp.ProductId);
        });

        modelBuilder.Entity<FavoriteProduct>()
        .HasKey(fp => new { fp.UserId, fp.ProductId });

        modelBuilder.Entity<FavoriteProduct>()
            .HasOne(fp => fp.User)
            .WithMany(u => u.FavoriteProducts)
            .HasForeignKey(fp => fp.UserId);

        modelBuilder.Entity<FavoriteProduct>()
            .HasOne(fp => fp.Product)
            .WithMany(p => p.FavoriteProducts)
            .HasForeignKey(fp => fp.ProductId);

        modelBuilder.Entity<OrderProduct>()
        .HasKey(op => new { op.OrderId, op.ProductId });

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Order)
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(op => op.OrderId);

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Product)
            .WithMany()
            .HasForeignKey(op => op.ProductId);
    }
}
