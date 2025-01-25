using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Business.Dtos;
using ShoppingApp.Business.Interfaces;
using ShoppingApp.Data.Entities;
using System.Threading.Tasks;

namespace ShoppingApp.WebApi.Controllers
{
    // Kullanıcılarla ilgili işlemleri sağlayan API kontrolcüsü
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService; // Kullanıcı servisinin bağımlılığı

        public UsersController(IUserService userService)
        {
            // Kullanıcı servisinin bağımlılığını başlatır
            _userService = userService;
        }

        // Admin yetkisine sahip kullanıcılar için özel veri sağlar
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdminData()
        {
            // Admin yetkisi doğrulanmış kullanıcılar için özel mesaj döner
            return Ok(new { Message = "Kullanıcı Admin yetkisine sahip." });
        }

        // Yeni bir kullanıcı kaydeder
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
        {
            // Gelen model doğrulanmazsa, hata mesajı döner
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kullanıcı oluşturma işlemini gerçekleştirir
            var result = await _userService.CreateUserAsync(new User
            {
                FirstName = userDto.FirstName, // Kullanıcının adı
                LastName = userDto.LastName, // Kullanıcının soyadı
                Email = userDto.Email, // Kullanıcının e-posta adresi
                PhoneNumber = userDto.PhoneNumber, // Kullanıcının telefon numarası
                Role = userDto.Role // Kullanıcının rolü
            }, userDto.Password);

            // Eğer işlem başarısızsa, hata mesajı döner
            if (!result.IsSucceed)
            {
                return BadRequest(new { Message = result.Message });
            }

            // İşlem başarılıysa, başarı mesajıyla cevap döner
            return Created("", new { Message = result.Message });
        }

        // Belirtilen ID'ye sahip kullanıcıyı getirir
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(int id)
        {
            // Token içindeki kullanıcı ID'sini alır
            var userIdClaim = User.FindFirst("id")?.Value;
            if (userIdClaim == null)
            {
                // Eğer token geçersizse, yetkisiz hatası döner
                return Unauthorized(new { Message = "Geçersiz token." });
            }

            var userId = int.Parse(userIdClaim);

            // Eğer kullanıcı admin değilse ve kendisinin bilgilerine erişmiyorsa yetkisiz hatası döner
            if (!User.IsInRole("Admin") && userId != id)
            {
                return Unauthorized(new { Message = "Sadece kendi bilgilerinize erişebilirsiniz." });
            }

            // Kullanıcı bilgilerini ID'ye göre alır
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                // Kullanıcı bulunamazsa hata mesajı döner
                return NotFound(new { Message = "Kullanıcı bulunamadı." });
            }

            // Kullanıcı bilgilerini DTO formatında döner
            var userDto = new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.PhoneNumber,
                Role = user.Role.ToString()
            };

            return Ok(userDto);
        }

        // Belirtilen ID'ye sahip bir kullanıcıyı tamamen günceller
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto userDto)
        {
            // Gelen model doğrulanmazsa, hata mesajı döner
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Token içindeki mevcut kullanıcı ID'sini alır
            var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");

            // Eğer kullanıcı admin değilse ve kendi bilgilerini güncellemiyorsa yetkisiz hatası döner
            if (currentUserId != id && !User.IsInRole("Admin"))
            {
                return Unauthorized(new { Message = "Sadece kendi bilgilerinizi güncelleyebilirsiniz veya admin olmalısınız." });
            }

            // Kullanıcı güncelleme işlemini gerçekleştirir
            var result = await _userService.UpdateUserAsync(new User
            {
                Id = id, // Güncellenen kullanıcının ID'si
                FirstName = userDto.FirstName, // Güncellenen ad
                LastName = userDto.LastName, // Güncellenen soyad
                Email = userDto.Email, // Güncellenen e-posta adresi
                PhoneNumber = userDto.PhoneNumber // Güncellenen telefon numarası
            }, id, userDto.Password);

            // Eğer işlem başarısızsa hata mesajı döner
            if (!result.IsSucceed)
            {
                return NotFound(new { Message = result.Message });
            }

            // İşlem başarılıysa başarı mesajı döner
            return Ok(new { Message = result.Message });
        }

        // Belirtilen ID'ye sahip bir kullanıcıyı kısmen günceller
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUser(int id, [FromBody] UserPatchModelDto userDto)
        {
            // Gelen model doğrulanmazsa hata mesajı döner
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kullanıcı kısmi güncelleme işlemini gerçekleştirir
            var result = await _userService.PatchUserAsync(id, userDto);

            // Eğer işlem başarısızsa hata mesajı döner
            if (!result.IsSucceed)
            {
                return NotFound(new { Message = result.Message });
            }

            // İşlem başarılıysa başarı mesajı döner
            return Ok(new { Message = result.Message });
        }

        // Belirtilen ID'ye sahip bir kullanıcıyı siler
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Kullanıcı silme işlemini gerçekleştirir
            var result = await _userService.DeleteUserAsync(id);

            // Eğer işlem başarısızsa hata mesajı döner
            if (!result.IsSucceed)
            {
                return NotFound(new { Message = result.Message });
            }

            // İşlem başarılıysa, 204 (No Content) durum koduyla döner
            return NoContent();
        }
    }
}