using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Business.Interfaces;
using ShoppingApp.Data.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ShoppingApp.Business.Dtos;

namespace ShoppingApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;

        public UsersController(IUserService userService, IPasswordHasher passwordHasher)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdminData()
        {
            return Ok(new { message = "Kullanıcı Admin yetkisine sahip." });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new { Message = "Kullanıcı detayları gereklidir." });
            }

            var createdUser = await _userService.CreateUserAsync(user, user.Password);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "Kullanıcı bulunamadı." });
            }

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest(new { Message = "Geçersiz kullanıcı bilgileri." });
            }

            var currentUserId = int.Parse(User.Identity.Name);
            if (currentUserId != id && !User.IsInRole("Admin"))
            {
                return Unauthorized(new { Message = "Sadece kendi bilgilerinizi güncelleyebilirsiniz veya admin rolüne sahip olmalısınız." });
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "Kullanıcı bulunamadı." });
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.PhoneNumber = userDto.PhoneNumber;

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                user.Password = _passwordHasher.HashPassword(userDto.Password);
            }

            await _userService.UpdateUserAsync(user, id);
            return NoContent();
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUser(int id, [FromBody] UserUpdateDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest(new { Message = "Geçersiz kullanıcı bilgileri." });
            }

            var currentUserId = int.Parse(User.Identity.Name);
            if (currentUserId != id && !User.IsInRole("Admin"))
            {
                return Unauthorized(new { Message = "Sadece kendi bilgilerinizi güncelleyebilirsiniz veya admin rolüne sahip olmalısınız." });
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "Kullanıcı bulunamadı." });
            }

            if (!string.IsNullOrEmpty(userDto.FirstName))
            {
                user.FirstName = userDto.FirstName;
            }

            if (!string.IsNullOrEmpty(userDto.LastName))
            {
                user.LastName = userDto.LastName;
            }

            if (!string.IsNullOrEmpty(userDto.Email))
            {
                user.Email = userDto.Email;
            }

            if (!string.IsNullOrEmpty(userDto.PhoneNumber))
            {
                user.PhoneNumber = userDto.PhoneNumber;
            }

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                user.Password = _passwordHasher.HashPassword(userDto.Password);
            }

            await _userService.UpdateUserAsync(user, id);
            return NoContent();
        }



        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Kullanıcı bulunamadı." });
            }
        }
    }
}