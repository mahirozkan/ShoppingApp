using Microsoft.EntityFrameworkCore;
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
    public class OrderService : IOrderService
    {
        private readonly ShoppingAppDbContext _context;

        public OrderService(ShoppingAppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceMessage> CreateOrderAsync(CreateOrderDto orderDto)
        {
            var order = new Order
            {
                OrderDate = orderDto.OrderDate,
                TotalAmount = orderDto.TotalAmount,
                CustomerId = orderDto.CustomerId,
                OrderProducts = orderDto.Products.Select(p => new OrderProduct
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity
                }).ToList()
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Sipariş başarıyla oluşturuldu."
            };
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders
                .Where(o => o.Id == orderId)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    CustomerId = o.CustomerId,
                    Products = o.OrderProducts.Select(op => new OrderProductDto
                    {
                        ProductId = op.ProductId,
                        ProductName = op.Product.ProductName,
                        Quantity = op.Quantity
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return order;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    CustomerId = o.CustomerId,
                    Products = o.OrderProducts.Select(op => new OrderProductDto
                    {
                        ProductId = op.ProductId,
                        ProductName = op.Product.ProductName,
                        Quantity = op.Quantity
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<ServiceMessage> UpdateOrderAsync(int orderId, UpdateOrderDto orderDto)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Sipariş bulunamadı."
                };
            }

            order.OrderDate = orderDto.OrderDate;
            order.TotalAmount = orderDto.TotalAmount;
            order.CustomerId = orderDto.CustomerId;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Sipariş başarıyla güncellendi."
            };
        }

        public async Task<ServiceMessage> DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Sipariş bulunamadı."
                };
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Sipariş başarıyla silindi."
            };
        }
    }
}
