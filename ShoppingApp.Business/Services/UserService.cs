using Microsoft.EntityFrameworkCore;
using ShoppingApp.Data.Entities;
using ShoppingApp.Data.Context;
using ShoppingApp.Business.Interfaces;
using System;
using System.Threading.Tasks;
using ShoppingApp.Business.Types;
using ShoppingApp.Business.Dtos;

namespace ShoppingApp.Business.Services
{
    public class UserService : IUserService
    {
        private readonly ShoppingAppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(ShoppingAppDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> CreateUserAsync(User user, string password)
        {
            user.Password = _passwordHasher.HashPassword(password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateUserAsync(User user, int currentUserId, string newPassword = null)
        {
            if (user.Id != currentUserId)
            {
                throw new UnauthorizedAccessException("You can only update your own account.");
            }

            if (!string.IsNullOrEmpty(newPassword))
            {
                user.Password = _passwordHasher.HashPassword(newPassword);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            if (user == null || !_passwordHasher.VerifyPassword(user.Password, password))
                return null;

            return user;
        }

        public async Task<ServiceMessage<UserInfoDto>> LoginUserAsync(LoginUserDto user)
        {
            var userEntity = await _context.Users
                .SingleOrDefaultAsync(x => x.Email.ToLower() == user.Email.ToLower());

            if (userEntity == null)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "Kullanıcı adı veya şifre hatalı!"
                };
            }

            if (!_passwordHasher.VerifyPassword(userEntity.Password, user.Password))
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "Kullanıcı adı veya şifre hatalı!"
                };
            }

            return new ServiceMessage<UserInfoDto>
            {
                IsSucceed = true,
                Data = new UserInfoDto
                {
                    Email = userEntity.Email,
                    FirstName = userEntity.FirstName,
                    LastName = userEntity.LastName,
                    Role = userEntity.Role,
                }
            };
        }
    }
}
