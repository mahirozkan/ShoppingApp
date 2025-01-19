using Microsoft.EntityFrameworkCore;
using ShoppingApp.Business.Dtos;
using ShoppingApp.Business.Interfaces;
using ShoppingApp.Data.Context;
using ShoppingApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto orderDto)
        {
            var order = new Order
            {
                CustomerId = orderDto.UserId,
                OrderDate = DateTime.UtcNow,
                OrderProducts = orderDto.ProductIds.Select(id => new OrderProduct { ProductId = id }).ToList(),
                TotalAmount = 0 // Toplam tutar hesaplaması yapılabilir.
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.CustomerId,
                OrderDate = order.OrderDate,
                ProductIds = orderDto.ProductIds,
                TotalAmount = order.TotalAmount
            };
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return null;

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.CustomerId,
                OrderDate = order.OrderDate,
                ProductIds = order.OrderProducts.Select(op => op.ProductId).ToList(),
                TotalAmount = order.TotalAmount
            };
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderProducts)
                .ToListAsync();

            return orders.Select(order => new OrderDto
            {
                Id = order.Id,
                UserId = order.CustomerId,
                OrderDate = order.OrderDate,
                ProductIds = order.OrderProducts.Select(op => op.ProductId).ToList(),
                TotalAmount = order.TotalAmount
            });
        }

        public async Task UpdateOrderAsync(UpdateOrderDto orderDto)
        {
            var order = await _context.Orders.Include(o => o.OrderProducts).FirstOrDefaultAsync(o => o.Id == orderDto.Id);
            if (order == null)
                throw new KeyNotFoundException();

            order.OrderProducts = orderDto.ProductIds.Select(id => new OrderProduct { OrderId = order.Id, ProductId = id }).ToList();
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
