using Microsoft.EntityFrameworkCore;
using ShoppingApp.Business.Dtos;
using ShoppingApp.Business.Interfaces;
using ShoppingApp.Data.Context;
using ShoppingApp.Data.Entities;

namespace ShoppingApp.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly ShoppingAppDbContext _context;

        public ProductService(ShoppingAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> CreateProductAsync(ProductCreateModelDto model)
        {
            var product = new Product
            {
                ProductName = model.ProductName,
                Price = model.Price,
                StockQuantity = model.StockQuantity
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task UpdateProductAsync(int id, ProductUpdateModelDto model)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.ProductName = model.ProductName;
                product.Price = model.Price;
                product.StockQuantity = model.StockQuantity;

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
