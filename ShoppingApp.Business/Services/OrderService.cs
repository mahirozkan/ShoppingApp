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
    // Sipariş hizmetlerinin implementasyonunu sağlayan sınıf
    public class OrderService : IOrderService
    {
        private readonly ShoppingAppDbContext _context; // Veri tabanı bağlamı

        public OrderService(ShoppingAppDbContext context)
        {
            // Veri tabanı bağlamını başlatır.
            _context = context;
        }

        // Yeni bir sipariş oluşturur.
        public async Task<ServiceMessage> CreateOrderAsync(CreateOrderDto orderDto)
        {
            var order = new Order
            {
                OrderDate = orderDto.OrderDate, // Sipariş tarihi
                TotalAmount = orderDto.TotalAmount, // Sipariş toplam tutarı
                CustomerId = orderDto.CustomerId, // Siparişi veren müşteri ID'si
                OrderProducts = orderDto.Products.Select(p => new OrderProduct
                {
                    ProductId = p.ProductId, // Ürünün ID'si
                    Quantity = p.Quantity // Sipariş edilen miktar
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

        // Belirtilen ID'ye göre bir siparişi getirir.
        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders
                .Where(o => o.Id == orderId)
                .Select(o => new OrderDto
                {
                    Id = o.Id, // Sipariş ID'si
                    OrderDate = o.OrderDate, // Sipariş tarihi
                    TotalAmount = o.TotalAmount, // Sipariş toplam tutarı
                    CustomerId = o.CustomerId, // Müşteri ID'si
                    Products = o.OrderProducts.Select(op => new OrderProductDto
                    {
                        ProductId = op.ProductId, // Ürün ID'si
                        ProductName = op.Product.ProductName, // Ürün adı
                        Quantity = op.Quantity // Sipariş edilen miktar
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return order;
        }

        // Tüm siparişleri getirir.
        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Select(o => new OrderDto
                {
                    Id = o.Id, // Sipariş ID'si
                    OrderDate = o.OrderDate, // Sipariş tarihi
                    TotalAmount = o.TotalAmount, // Sipariş toplam tutarı
                    CustomerId = o.CustomerId, // Müşteri ID'si
                    Products = o.OrderProducts.Select(op => new OrderProductDto
                    {
                        ProductId = op.ProductId, // Ürün ID'si
                        ProductName = op.Product.ProductName, // Ürün adı
                        Quantity = op.Quantity // Sipariş edilen miktar
                    }).ToList()
                })
                .ToListAsync();
        }

        // Belirtilen ID'ye göre bir siparişi günceller.
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

            // Sipariş bilgilerini günceller.
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

        // Belirtilen ID'ye göre bir siparişi siler.
        public async Task<ServiceMessage> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderProducts).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Sipariş bulunamadı."
                };
            }

            // İlişkili ürünleri siler.
            _context.OrderProducts.RemoveRange(order.OrderProducts);

            // Siparişi siler.
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