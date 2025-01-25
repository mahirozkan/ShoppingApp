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
    public class ProductService : IProductService
    {
        private readonly ShoppingAppDbContext _context;

        public ProductService(ShoppingAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            return _context.Products
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity
                })
                .ToList();
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return null;

            return new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            };
        }

        public async Task<ServiceMessage> CreateProductAsync(ProductCreateModelDto model)
        {
            var product = new Product
            {
                ProductName = model.ProductName,
                Price = model.Price,
                StockQuantity = model.StockQuantity
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Ürün başarıyla oluşturuldu."
            };
        }

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
    }
}
