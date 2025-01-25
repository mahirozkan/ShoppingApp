using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Business.Dtos;
using ShoppingApp.Business.Interfaces;
using ShoppingApp.WebApi.Filters;
using ShoppingApp.WebApi.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoppingApp.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
                return NotFound(new { Message = "Sipariş bulunamadı." });

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(User.FindFirst(JwtClaimNames.Id)?.Value);

            if (userRole == "Customer" && order.CustomerId != userId)
            {
                return Forbid();
            }

            return Ok(order);
        }

        [HttpPost]
        [ServiceFilter(typeof(TimeBasedAuthorizationFilter))]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _orderService.CreateOrderAsync(orderDto);

            if (!result.IsSucceed)
            {
                return BadRequest(new { Message = result.Message });
            }

            return Created("", new { Message = result.Message });
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(TimeBasedAuthorizationFilter))]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Geçersiz model verisi.", Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            var userId = int.Parse(User.FindFirst(JwtClaimNames.Id)?.Value);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var existingOrder = await _orderService.GetOrderByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound(new { Message = "Sipariş bulunamadı." });
            }

            if (userRole != "Admin" && existingOrder.CustomerId != userId)
            {
                return BadRequest(new { Message = "Bu siparişi güncelleme yetkiniz yok." });
            }

            var result = await _orderService.UpdateOrderAsync(id, orderDto);

            if (!result.IsSucceed)
            {
                return BadRequest(new { Message = "Sipariş güncellenirken bir hata oluştu.", Detail = result.Message });
            }

            return Ok(new { Message = result.Message });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);

            if (!result.IsSucceed)
            {
                return NotFound(new { Message = result.Message });
            }

            return Ok(new { Message = result.Message });
        }
    }
}
