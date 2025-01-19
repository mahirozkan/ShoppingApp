using Microsoft.EntityFrameworkCore;
using ShoppingApp.Data.Entities;
using ShoppingApp.Data.Context;
using ShoppingApp.Business.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using ShoppingApp.Business.Types;
using ShoppingApp.Business.Dtos;
using ShoppingApp.Data.Repositories;

namespace ShoppingApp.Business.Services
{
    public class UserService : IUserService
    {
        private readonly ShoppingAppDbContext _context;
        private readonly IRepository<User> _userRepository;
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

        public async Task UpdatePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            if (!_passwordHasher.VerifyPassword(user.Password, oldPassword))
            {
                throw new UnauthorizedAccessException("Old password is incorrect.");
            }

            user.Password = _passwordHasher.HashPassword(newPassword);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user)
        {
            var userEntity = _userRepository.Get(x => x.Email.ToLower() == user.Email.ToLower());

            if (userEntity == null)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "Kullanıcı adı veya şifre hatalı!"
                };
            }

            var unprotectedPassword = _passwordHasher.HashPassword(userEntity.Password);

            if (unprotectedPassword == user.Password)
            {
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

            else
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "Kullanıcı adı veya şifre hatalı!"
                };
            }
        }
    }
}

