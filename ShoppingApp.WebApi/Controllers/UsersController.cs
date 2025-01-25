using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Business.Dtos;
using ShoppingApp.Business.Interfaces;
using ShoppingApp.Data.Entities;
using System.Threading.Tasks;

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
            return Ok(new { Message = "Kullanıcı Admin yetkisine sahip." });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.CreateUserAsync(new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                Role = userDto.Role
            }, userDto.Password);

            if (!result.IsSucceed)
            {
                return BadRequest(new { Message = result.Message });
            }

            return Created("", new { Message = result.Message });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(int id)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized(new { Message = "Geçersiz token." });
            }

            var userId = int.Parse(userIdClaim);

            if (!User.IsInRole("Admin") && userId != id)
            {
                return Unauthorized(new { Message = "Sadece kendi bilgilerinize erişebilirsiniz." });
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "Kullanıcı bulunamadı." });
            }

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


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            if (currentUserId != id && !User.IsInRole("Admin"))
            {
                return Unauthorized(new { Message = "Sadece kendi bilgilerinizi güncelleyebilirsiniz veya admin olmalısınız." });
            }

            var result = await _userService.UpdateUserAsync(new User
            {
                Id = id,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber
            }, id, userDto.Password);

            if (!result.IsSucceed)
            {
                return NotFound(new { Message = result.Message });
            }

            return Ok(new { Message = result.Message });
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUser(int id, [FromBody] UserPatchModelDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.PatchUserAsync(id, userDto);

            if (!result.IsSucceed)
            {
                return NotFound(new { Message = result.Message });
            }

            return Ok(new { Message = result.Message });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);

            if (!result.IsSucceed)
            {
                return NotFound(new { Message = result.Message });
            }

            return NoContent();
        }

    }
}
