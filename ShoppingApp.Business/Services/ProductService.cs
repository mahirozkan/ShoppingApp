﻿using Microsoft.EntityFrameworkCore;
using ShoppingApp.Business.Dtos;
using ShoppingApp.Business.Interfaces;
using ShoppingApp.Business.Types;
using ShoppingApp.Data.Context;
using ShoppingApp.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Services
{
    // Ürün hizmetlerinin implementasyonunu sağlayan sınıf
    public class ProductService : IProductService
    {
        private readonly ShoppingAppDbContext _context; // Veri tabanı bağlamı

        public ProductService(ShoppingAppDbContext context)
        {
            // Veri tabanı bağlamını başlatır.
            _context = context;
        }

        // Tüm ürünleri getirir.
        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            return _context.Products
                .Select(p => new ProductDto
                {
                    Id = p.Id, // Ürün ID'si
                    ProductName = p.ProductName, // Ürün adı
                    Price = p.Price, // Ürün fiyatı
                    StockQuantity = p.StockQuantity // Stok miktarı
                })
                .ToList();
        }

        // Belirtilen ID'ye göre bir ürünü getirir.
        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return null; // Ürün bulunamazsa null döner.

            return new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            };
        }

        // Yeni bir ürün oluşturur.
        public async Task<ServiceMessage> CreateProductAsync(ProductCreateModelDto model)
        {
            var product = new Product
            {
                ProductName = model.ProductName, // Ürün adı
                Price = model.Price, // Ürün fiyatı
                StockQuantity = model.StockQuantity // Stok miktarı
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Ürün başarıyla oluşturuldu."
            };
        }

        // Belirtilen ID'ye göre bir ürünü tamamen günceller.
        public async Task<ServiceMessage> UpdateProductAsync(int id, ProductUpdateModelDto model)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Ürün bulunamadı."
                };
            }

            // Ürün bilgilerini günceller.
            product.ProductName = model.ProductName;
            product.Price = model.Price;
            product.StockQuantity = model.StockQuantity;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Ürün başarıyla güncellendi."
            };
        }

        // Belirtilen ID'ye göre bir ürünü kısmi olarak günceller.
        public async Task<ServiceMessage> PatchProductAsync(int id, ProductPatchModelDto productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Ürün bulunamadı."
                };
            }

            // Alan güncellemeleri
            if (!string.IsNullOrEmpty(productDto.ProductName))
            {
                product.ProductName = productDto.ProductName;
            }

            if (productDto.Price.HasValue)
            {
                product.Price = productDto.Price.Value;
            }

            if (productDto.StockQuantity.HasValue)
            {
                product.StockQuantity = productDto.StockQuantity.Value;
            }

            // Veritabanına kaydet
            await _context.SaveChangesAsync();

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Ürün başarıyla güncellendi."
            };
        }

        // Belirtilen ID'ye göre bir ürünü siler.
        public async Task<ServiceMessage> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Ürün bulunamadı."
                };
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Ürün başarıyla silindi."
            };
        }

        public async Task<PagedResult<ProductDto>> GetPagedProductsAsync(int page, int pageSize)
        {
            var query = _context.Products.AsQueryable(); // Veritabanındaki tüm ürünlere bir sorgu başlatıyoruz.
            var totalCount = await query.CountAsync(); // Toplam ürün sayısını alıyoruz.

            // Belirtilen sayfa numarasına göre ürünleri atlayıp (Skip) yalnızca istenen sayıda (Take) ürün alıyoruz.
            var items = await query.Skip((page - 1) * pageSize) // Örneğin, sayfa 2 ve sayfa büyüklüğü 10 ise ilk 10 öğeyi atlar.
                .Take(pageSize) // Sayfa büyüklüğüne göre yalnızca belirli sayıda öğe alır.
                .Select(p => new ProductDto 
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity
                })
                .ToListAsync();

            // Sonuçları PagedResult sınıfı ile döndürüyoruz.
            return new PagedResult<ProductDto>
            {
                CurrentPage = page, // Şu anki sayfa
                PageSize = pageSize, // Her sayfadaki öğe sayısı
                TotalCount = totalCount, // Toplam ürün sayısı
                Items = items // Sayfaya ait ürünler
            };
        }
    }
}