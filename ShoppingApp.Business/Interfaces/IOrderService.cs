using ShoppingApp.Business.Dtos;
using ShoppingApp.Business.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Interfaces
{
    public interface IOrderService
    {
        Task<ServiceMessage> CreateOrderAsync(CreateOrderDto orderDto);
        Task<OrderDto> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<ServiceMessage> UpdateOrderAsync(int orderId, UpdateOrderDto orderDto);
        Task<ServiceMessage> DeleteOrderAsync(int orderId);
    }
}
