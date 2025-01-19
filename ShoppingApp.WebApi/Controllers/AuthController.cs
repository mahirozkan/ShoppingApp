﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Business.Interfaces;
using ShoppingApp.Data.Entities;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using ShoppingApp.WebApi.Jwt;
using ShoppingApp.Business.Dtos;
using Microsoft.AspNetCore.Identity.Data;

namespace ShoppingApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Password is required");
            }

            try
            {
                var createdUser = await _userService.CreateUserAsync(user, user.Password);

                return Ok(new { Message = "Registration successful", User = createdUser });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while registering the user.", Details = ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _userService.LoginUser(new LoginUserDto { Email = request.Email, Password = request.Password });

            if (!result.IsSucceed)
            {
                return BadRequest(result.Message);
            }

            var user = result.Data;

            var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var token = JwtHelper.GenerateJwtToken(new JwtDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                SecretKey = configuration["Jwt:SecretKey"]!,
                Issuer = configuration["Jwt:Issuer"]!,
                Audience = configuration["Jwt:Audience"]!,
                ExpiryInMinutes = int.Parse(configuration["Jwt:ExpireMinutes"]!)
            });

            return Ok(new
            {
                Message = "An error occurred while registering the user.",
                Token = token
            });
        }
    }
}

