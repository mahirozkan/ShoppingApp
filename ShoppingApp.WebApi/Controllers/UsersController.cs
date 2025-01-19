using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Business.Interfaces;
using ShoppingApp.Data.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ShoppingApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdminData()
        {
            return Ok(new { message = "This is an Admin data." });
        }
        [HttpPost]
        [AllowAnonymous] 
        public async Task<IActionResult> CreateUser([FromBody] User user, [FromQuery] string password)
        {
            if (user == null || string.IsNullOrEmpty(password))
            {
                return BadRequest("Kullanıcı bilgileri ve şifre gereklidir.");
            }

            var createdUser = await _userService.CreateUserAsync(user, password);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user, [FromQuery] string newPassword = null)
        {
            if (user == null || user.Id != id)
            {
                return BadRequest("Geçersiz kullanıcı bilgileri.");
            }

            try
            {
                await _userService.UpdateUserAsync(user, id, newPassword);
                return NoContent(); 
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Kendi bilgilerinizi yalnızca güncelleyebilirsiniz.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }
        }

        [HttpPost("authenticate")]
        [AllowAnonymous] 
        public async Task<IActionResult> Authenticate([FromQuery] string email, [FromQuery] string password)
        {
            var user = await _userService.AuthenticateAsync(email, password);
            if (user == null)
            {
                return Unauthorized("Geçersiz e-posta veya şifre.");
            }

            return Ok(user);
        }
    }
}
