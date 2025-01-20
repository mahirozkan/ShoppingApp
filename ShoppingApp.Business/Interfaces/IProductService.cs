using ShoppingApp.Business.Dtos;
using ShoppingApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(ProductCreateModelDto model);
        Task UpdateProductAsync(int id, ProductUpdateModelDto model);
        Task DeleteProductAsync(int id);
    }
}
