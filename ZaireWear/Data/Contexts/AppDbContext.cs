﻿using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contexts
{
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
                .HasKey(bp => new { bp.BasketId, bp.ProductId });

            modelBuilder.Entity<BasketProduct>()
                .HasOne(bp => bp.Basket)
                .WithMany(b => b.BasketProducts)
                .HasForeignKey(bp => bp.BasketId);

            modelBuilder.Entity<BasketProduct>()
                .HasOne(bp => bp.Product)
                .WithMany(p => p.BasketProducts)
                .HasForeignKey(bp => bp.ProductId);

            modelBuilder.Entity<Basket>()
                .HasOne(b => b.User)
                .WithOne(u => u.Basket)
                .HasForeignKey<Basket>(b => b.UserId);

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
        }
    }
}
