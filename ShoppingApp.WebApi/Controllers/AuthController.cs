using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShoppingApp.Business.Dtos;
using ShoppingApp.Business.Interfaces;
using ShoppingApp.Data.Entities;
using ShoppingApp.WebApi.Jwt;
using System.Threading.Tasks;

namespace ShoppingApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
        {
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Role = userDto.Role
            };

            var result = await _userService.CreateUserAsync(user, userDto.Password);

            if (!result.IsSucceed)
            {
                return BadRequest(new { Message = result.Message });
            }

            return Ok(new { Message = result.Message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            var result = await _userService.LoginUserAsync(loginDto);

            if (!result.IsSucceed)
            {
                return Unauthorized(new { Message = result.Message });
            }

            var jwtInfo = new JwtDto
            {
                Id = result.Data.Id,
                Email = result.Data.Email,
                FirstName = result.Data.FirstName,
                LastName = result.Data.LastName,
                Role = result.Data.Role,
                SecretKey = _configuration["Jwt:SecretKey"],
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                ExpiryInMinutes = int.Parse(_configuration["Jwt:ExpiryInMinutes"])
            };

            var token = JwtHelper.GenerateJwtToken(jwtInfo);

            return Ok(new
            {
                Message = "Giriş başarılı.",
                Token = token,
                Data = result.Data
            });
        }

    }
}
