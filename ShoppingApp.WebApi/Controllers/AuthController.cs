using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Business.Dtos;
using ShoppingApp.Business.Interfaces;
using ShoppingApp.Data.Entities;
using ShoppingApp.WebApi.Jwt;

[ApiController] // Bu sınıfın bir API denetleyicisi olduğunu belirtir
[Route("api/[controller]")] // Controller için rota tanımı: "api/auth"
public class AuthController : ControllerBase
{
    private readonly IUserService _userService; // Kullanıcı işlemleri servisi
    private readonly IConfiguration _configuration; // Konfigürasyon ayarlarını okuyabilmek için kullanılır

    // Constructor: Servisler dependency injection ile enjekte edilir
    public AuthController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    // Kullanıcı kaydı için endpoint
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
    {
        // Gelen model geçersizse, hata mesajı döner
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Yeni bir kullanıcı nesnesi oluşturulur
        var user = new User
        {
            FirstName = userDto.FirstName, // Kullanıcının adı
            LastName = userDto.LastName, // Kullanıcının soyadı
            Email = userDto.Email, // Kullanıcının e-posta adresi
            PhoneNumber = userDto.PhoneNumber, // Kullanıcının telefon numarası
            Role = userDto.Role // Kullanıcının rolü
        };

        // Kullanıcı oluşturma işlemi yapılır
        var result = await _userService.CreateUserAsync(user, userDto.Password);

        // Eğer işlem başarısızsa hata mesajı döner
        if (!result.IsSucceed)
        {
            return BadRequest(new { Message = result.Message });
        }

        // İşlem başarılıysa başarı mesajı döner
        return Ok(new { Message = result.Message });
    }

    // Kullanıcı girişi için endpoint
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
    {
        // Gelen model doğrulanmazsa hata mesajı döner
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Kullanıcı giriş işlemi yapılır
            var result = await _userService.LoginUserAsync(loginDto);

            // Giriş başarısızsa yetkisiz hata mesajı döner
            if (!result.IsSucceed)
            {
                return Unauthorized(new { Message = "Geçersiz kullanıcı adı veya şifre." });
            }

            // JWT token bilgilerini oluşturmak için gerekli veriler hazırlanır
            var jwtInfo = new JwtDto
            {
                Id = result.Data.Id, // Kullanıcı ID
                Email = result.Data.Email, // Kullanıcı e-postası
                FirstName = result.Data.FirstName, // Kullanıcı adı
                LastName = result.Data.LastName, // Kullanıcı soyadı
                Role = result.Data.Role, // Kullanıcı rolü
                SecretKey = _configuration["Jwt:SecretKey"], // JWT için gizli anahtar
                Issuer = _configuration["Jwt:Issuer"], // Token sağlayıcı bilgisi
                Audience = _configuration["Jwt:Audience"], // Token hedef kitlesi
                ExpiryInMinutes = int.Parse(_configuration["Jwt:ExpiryInMinutes"]) // Token geçerlilik süresi
            };

            // JWT token oluşturulur
            var token = JwtHelper.GenerateJwtToken(jwtInfo);

            // Giriş başarılıysa, token ve kullanıcı bilgileri döndürülür
            return Ok(new
            {
                Message = "Giriş başarılı.",
                Token = token, // Oluşturulan JWT token
                Data = result.Data // Kullanıcı bilgileri
            });
        }
        catch (Exception ex)
        {
            // Bir hata oluşursa, 500 durum koduyla hata bilgisi döndürülür
            return StatusCode(500, new { Message = "Bir hata oluştu.", Detail = ex.Message });
        }
    }
}
