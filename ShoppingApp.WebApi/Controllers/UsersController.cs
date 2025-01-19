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

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdminData()
        {
            return Ok(new { message = "This is Admin data." });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new { Message = "User details and password are required." });
            }

            var createdUser = await _userService.CreateUserAsync(user, user.Password);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user, [FromQuery] string newPassword = null)
        {
            if (user == null || user.Id != id)
            {
                return BadRequest(new { Message = "Invalid user data." });
            }

            try
            {
                await _userService.UpdateUserAsync(user, id, newPassword);
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { Message = "You can only update your own data." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "User not found." });
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
                return NotFound(new { Message = "User not found." });
            }
        }
    }
}
