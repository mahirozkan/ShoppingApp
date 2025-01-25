using ShoppingApp.Business.Dtos; // DTO'ları içeren namespace
using ShoppingApp.Business.Types; // Hizmet mesajı tanımlarını içeren namespace
using System.Collections.Generic; // List koleksiyonunu kullanabilmek için gerekli namespace
using System.Threading.Tasks; // Asenkron operasyonlar için gerekli namespace

namespace ShoppingApp.Business.Interfaces
{
    // Ürün hizmetlerine ilişkin metotları tanımlayan arayüz
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync(); // Tüm ürünleri listeler.

        Task<ProductDto> GetProductByIdAsync(int id); // Belirli bir ID'ye sahip ürünün detaylarını getirir.

        Task<ServiceMessage> CreateProductAsync(ProductCreateModelDto model); // Yeni bir ürün oluşturur.

        Task<ServiceMessage> UpdateProductAsync(int id, ProductUpdateModelDto model); // Belirtilen ID'ye sahip ürünün tüm detaylarını günceller.

        Task<ServiceMessage> PatchProductAsync(int id, ProductPatchModelDto model); // Belirtilen ID'ye sahip ürünün kısmi detaylarını günceller.

        Task<ServiceMessage> DeleteProductAsync(int id); // Belirtilen ID'ye sahip ürünün silinmesini sağlar.
    }
}
