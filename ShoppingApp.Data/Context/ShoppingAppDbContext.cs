using ShoppingApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ShoppingApp.Data.Context
{
    // Uygulama için veri tabanı bağlamını temsil eden sınıf
    public class ShoppingAppDbContext : DbContext
    {
        public ShoppingAppDbContext(DbContextOptions<ShoppingAppDbContext> options) : base(options)
        {
        }

        // Kullanıcılar tablosu
        public DbSet<User> Users { get; set; }
        // Ürünler tablosu
        public DbSet<Product> Products { get; set; }
        // Siparişler tablosu
        public DbSet<Order> Orders { get; set; }
        // Sipariş-Ürün ilişkisini temsil eden tablo
        public DbSet<OrderProduct> OrderProducts { get; set; }

        // Veri tabanı yapılandırmasını ayarlamak için kullanılır
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Kullanıcıların e-posta adresi benzersiz olmalı
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Kullanıcı adının maksimum uzunluğu 50 ve zorunlu
            modelBuilder.Entity<User>()
                .Property(u => u.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            // Kullanıcı soyadının maksimum uzunluğu 50 ve zorunlu
            modelBuilder.Entity<User>()
                .Property(u => u.LastName)
                .HasMaxLength(50)
                .IsRequired();

            // Kullanıcı e-postasının maksimum uzunluğu 100 ve zorunlu
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(100)
                .IsRequired();

            // Kullanıcı rolünün string olarak saklanması
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>()
                .HasDefaultValue(Role.Customer); // Varsayılan rol "Customer"

            // OrderProduct tablosunda birincil anahtar tanımı
            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            // OrderProduct ve Order arasındaki ilişki
            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Sipariş silindiğinde ilişkili ürünler de silinir

            // OrderProduct ve Product arasındaki ilişki
            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Ürün silinemezse ilişkiler bozulmaz

            // Ürün adının benzersiz olması
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.ProductName)
                .IsUnique();

            // Ürün fiyatının ondalık veri türünde tanımlanması
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            // Sipariş toplam tutarının ondalık veri türünde tanımlanması
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            // Sipariş ve Müşteri arasındaki ilişki
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); // Müşteri silinemezse sipariş bozulmaz
        }
    }
}
