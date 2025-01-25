using ShoppingApp.Business.Dtos; // DTO'ları içeren namespace
using ShoppingApp.Business.Types; // Hizmet mesajı tanımlarını içeren namespace
using System.Collections.Generic; // Koleksiyonları kullanabilmek için gerekli namespace
using System.Threading.Tasks; // Asenkron operasyonlar için gerekli namespace

namespace ShoppingApp.Business.Interfaces
{
    // Sipariş hizmetlerine ilişkin metotları tanımlayan arayüz
    public interface IOrderService
    {
        Task<ServiceMessage> CreateOrderAsync(CreateOrderDto orderDto); // Yeni bir sipariş oluşturur.

        Task<OrderDto> GetOrderByIdAsync(int orderId); // Belirli bir ID'ye sahip siparişin detaylarını getirir.

        Task<IEnumerable<OrderDto>> GetAllOrdersAsync(); // Tüm siparişleri listeler.

        Task<ServiceMessage> UpdateOrderAsync(int orderId, UpdateOrderDto orderDto); // Belirtilen ID'ye sahip siparişi günceller.

        Task<ServiceMessage> DeleteOrderAsync(int orderId); // Belirtilen ID'ye sahip siparişi siler.
    }
}
