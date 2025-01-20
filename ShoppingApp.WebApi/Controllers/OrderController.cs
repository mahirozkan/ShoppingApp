using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Business.Interfaces;
using ShoppingApp.Data.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ShoppingApp.Business.Dtos;

namespace ShoppingApp.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _orderService.CreateOrderAsync(orderDto);
            return CreatedAtAction(nameof(GetOrderById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound("Sipariş bulunamadı.");

            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDto orderDto)
        {
            if (id != orderDto.Id)
                return BadRequest("Geçersiz sipariş bilgisi.");

            try
            {
                await _orderService.UpdateOrderAsync(orderDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Sipariş bulunamadı.");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchOrder(int id, [FromBody] UpdateOrderDto orderDto)
        {
            if (id != orderDto.Id)
                return BadRequest("Geçersiz sipariş bilgilsi.");

            try
            {
                await _orderService.UpdateOrderAsync(orderDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Sipariş bulunamadı.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Sipariş bulunamadı.");
            }
        }
    }
}
