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
    // Kullanıcı hizmetlerinin implementasyonunu sağlayan sınıf
    public class UserService : IUserService
    {
        private readonly ShoppingAppDbContext _context; // Veri tabanı bağlamını temsil eder ve veritabanı işlemleri için kullanılır.
        private readonly IDataProtector _dataProtector; // Veri koruma için kullanılan bir araç.
        private readonly PasswordHasher<User> _passwordHasher; // Kullanıcı parolalarını hashlemek ve doğrulamak için kullanılır.

        public UserService(ShoppingAppDbContext context, IDataProtectionProvider dataProtectionProvider)
        {
            // Bağımlılıkları başlatır ve veri koruma için bir "protector" oluşturur.
            _context = context;
            _dataProtector = dataProtectionProvider.CreateProtector("ShoppingApp.Data.Protector");
            _passwordHasher = new PasswordHasher<User>();
        }

        // Yeni bir kullanıcı oluşturur.
        public async Task<ServiceMessage<User>> CreateUserAsync(User user, string password)
        {
            // E-posta adresine göre var olan bir kullanıcıyı kontrol eder.
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser != null)
            {
                // Eğer kullanıcı mevcutsa, hata mesajı döner.
                return new ServiceMessage<User>
                {
                    IsSucceed = false,
                    Message = "Bu e-posta adresi zaten kayıtlı."
                };
            }

            // Parolayı hashler ve kullanıcıyı veritabanına ekler.
            user.Password = _passwordHasher.HashPassword(user, password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new ServiceMessage<User>
            {
                IsSucceed = true,
                Message = "Kullanıcı başarıyla oluşturuldu.",
                Data = user
            };
        }

        // Belirtilen ID'ye göre bir kullanıcıyı getirir.
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        // Mevcut bir kullanıcıyı günceller ve isteğe bağlı olarak parolasını değiştirir.
        public async Task<ServiceMessage> UpdateUserAsync(User user, int currentUserId, string newPassword = null)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);

            if (existingUser == null)
            {
                // Kullanıcı bulunamazsa hata mesajı döner.
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Kullanıcı bulunamadı."
                };
            }

            // Kullanıcı bilgilerini günceller.
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;

            // Yeni parola sağlanmışsa hashlenmiş olarak kaydedilir.
            if (!string.IsNullOrEmpty(newPassword))
            {
                existingUser.Password = _passwordHasher.HashPassword(existingUser, newPassword);
            }

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Kullanıcı başarıyla güncellendi."
            };
        }

        // Belirtilen ID'ye sahip kullanıcıyı siler.
        public async Task<ServiceMessage> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                // Kullanıcı bulunamazsa hata mesajı döner.
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Kullanıcı bulunamadı."
                };
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Kullanıcı başarıyla silindi."
            };
        }

        // Kullanıcının kimlik doğrulamasını yapar.
        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, password) != PasswordVerificationResult.Success)
            {
                // Kullanıcı bulunamazsa veya parola hatalıysa null döner.
                return null;
            }

            return user;
        }

        // Kullanıcı giriş işlemini gerçekleştirir ve kullanıcı bilgilerini döner.
        public async Task<ServiceMessage<UserInfoDto>> LoginUserAsync(LoginUserDto userDto)
        {
            var user = await AuthenticateAsync(userDto.Email, userDto.Password);

            if (user == null)
            {
                // Geçersiz giriş durumunda hata mesajı döner.
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

        // Belirli bir kullanıcıyı kısmi olarak günceller.
        public async Task<ServiceMessage> PatchUserAsync(int id, UserPatchModelDto userDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                // Kullanıcı bulunamazsa hata mesajı döner.
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Kullanıcı bulunamadı."
                };
            }

            // Gelen alanlar boş değilse güncellemeleri uygular.
            if (!string.IsNullOrEmpty(userDto.FirstName))
                user.FirstName = userDto.FirstName;

            if (!string.IsNullOrEmpty(userDto.LastName))
                user.LastName = userDto.LastName;

            if (!string.IsNullOrEmpty(userDto.Email))
                user.Email = userDto.Email;

            if (!string.IsNullOrEmpty(userDto.PhoneNumber))
                user.PhoneNumber = userDto.PhoneNumber;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Kullanıcı başarıyla güncellendi."
            };
        }
    }
}
