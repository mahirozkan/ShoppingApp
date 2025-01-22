using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Business.Dtos;
using ShoppingApp.Business.Interfaces;
using ShoppingApp.Business.Types;
using ShoppingApp.Data.Context;
using ShoppingApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Services
{
    public class UserService : IUserService
    {
        private readonly ShoppingAppDbContext _context;
        private readonly IDataProtector _dataProtector;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(ShoppingAppDbContext context, IDataProtectionProvider dataProtectionProvider)
        {
            _context = context;
            _dataProtector = dataProtectionProvider.CreateProtector("ShoppingApp.Data.Protector");
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<User> CreateUserAsync(User user, string password)
        {
            user.Password = _passwordHasher.HashPassword(user, password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task UpdateUserAsync(User user, int currentUserId, string newPassword = null)
        {
            var existingUser = await GetUserByIdAsync(user.Id);

            if (existingUser == null)
                throw new KeyNotFoundException("Kullanıcı bulunamadı.");

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;

            if (!string.IsNullOrEmpty(newPassword))
            {
                existingUser.Password = _passwordHasher.HashPassword(existingUser, newPassword);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id);

            if (user == null)
                throw new KeyNotFoundException("Kullanıcı bulunamadı.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, password) != PasswordVerificationResult.Success)
            {
                return null;
            }

            return user;
        }

        public async Task<ServiceMessage<UserInfoDto>> LoginUserAsync(LoginUserDto userDto)
        {
            var user = await AuthenticateAsync(userDto.Email, userDto.Password);

            if (user == null)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "Geçersiz kullanıcı adı veya şifre."
                };
            }

            return new ServiceMessage<UserInfoDto>
            {
                IsSucceed = true,
                Message = "Giriş başarılı.",
                Data = new UserInfoDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Role = user.Role
                }
            };
        }
    }
}
