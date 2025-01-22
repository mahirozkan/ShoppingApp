using ShoppingApp.Business.Dtos;
using ShoppingApp.Business.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<ServiceMessage> CreateProductAsync(ProductCreateModelDto model);
        Task<ServiceMessage> UpdateProductAsync(int id, ProductUpdateModelDto model);
        Task<ServiceMessage> PatchProductAsync(int id, ProductPatchModelDto model);
        Task<ServiceMessage> DeleteProductAsync(int id);
    }
}
