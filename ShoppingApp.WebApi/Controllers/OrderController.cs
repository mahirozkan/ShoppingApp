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
    // Siparişlerle ilgili işlemleri gerçekleştiren API kontrolcüsü
    [Authorize] // Tüm işlemler için yetkilendirme gereklidir
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        // IOrderService bağımlılığı enjekte edilir
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Kullanıcının belirli bir siparişe erişim yetkisi olup olmadığını kontrol eden yardımcı metot
        private bool IsUserAuthorizedToAccessOrder(int orderCustomerId)
        {
            // Kullanıcının rolünü ve ID'sini JWT token'dan alır
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(User.FindFirst(JwtClaimNames.Id)?.Value);

            // Kullanıcı Admin ise veya kendi siparişine erişiyorsa yetkilendirme başarılı olur
            return userRole == "Admin" || userId == orderCustomerId;
        }

        // Tüm siparişleri listeleme (Sadece Admin erişebilir)
        [HttpGet]
        [Authorize(Roles = "Admin")] // Sadece Admin rolündeki kullanıcılar bu endpoint'e erişebilir
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync(); // Tüm siparişleri getirir
            return Ok(orders); // Siparişleri döndürür
        }

        // Belirli bir siparişi ID ile getirme
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id); // Siparişi ID ile bul

            if (order == null)
                return NotFound(new { Message = "Sipariş bulunamadı." }); // Sipariş yoksa hata döndür

            // Kullanıcı yetkisi kontrol edilir
            if (!IsUserAuthorizedToAccessOrder(order.CustomerId))
            {
                return BadRequest(new { Message = "Bu sipariş bilgilerine erişim yetkiniz yok." });
            }

            return Ok(order); // Yetki varsa sipariş bilgilerini döndür
        }

        // Yeni bir sipariş oluşturma
        [HttpPost]
        [ServiceFilter(typeof(TimeBasedAuthorizationFilter))] // Zaman bazlı yetkilendirme kontrolü uygular
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Model doğrulaması geçmezse hata döner
            }

            var result = await _orderService.CreateOrderAsync(orderDto); // Yeni sipariş oluştur

            if (!result.IsSucceed)
            {
                return BadRequest(new { Message = result.Message }); // İşlem başarısızsa hata mesajı döner
            }

            return Created("", new { Message = result.Message }); // Başarı durumunda mesaj döner
        }

        // Belirli bir siparişi güncelleme
        [HttpPut("{id}")]
        [ServiceFilter(typeof(TimeBasedAuthorizationFilter))] // Zaman bazlı yetkilendirme kontrolü uygular
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Geçersiz model verisi.", Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            var existingOrder = await _orderService.GetOrderByIdAsync(id); // Mevcut siparişi bul
            if (existingOrder == null)
            {
                return NotFound(new { Message = "Sipariş bulunamadı." }); // Sipariş yoksa hata döner
            }

            // Kullanıcı yetkisi kontrol edilir
            if (!IsUserAuthorizedToAccessOrder(existingOrder.CustomerId))
            {
                return BadRequest(new { Message = "Bu siparişi güncelleme yetkiniz yok." });
            }

            var result = await _orderService.UpdateOrderAsync(id, orderDto); // Siparişi güncelle

            if (!result.IsSucceed)
            {
                return BadRequest(new { Message = "Sipariş güncellenirken bir hata oluştu.", Detail = result.Message });
            }

            return Ok(new { Message = result.Message }); // Güncelleme başarılıysa mesaj döner
        }

        // Belirli bir siparişi silme (Sadece Admin erişebilir)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Sadece Admin rolündeki kullanıcılar bu endpoint'e erişebilir
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id); // Siparişi sil

            if (!result.IsSucceed)
            {
                return NotFound(new { Message = result.Message }); // İşlem başarısızsa hata mesajı döner
            }

            return Ok(new { Message = result.Message }); // İşlem başarılıysa mesaj döner
        }
    }
}
