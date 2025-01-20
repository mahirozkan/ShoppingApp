using Microsoft.EntityFrameworkCore;
using ShoppingApp.Business.Dtos;
using ShoppingApp.Business.Interfaces;
using ShoppingApp.Business.Types;
using ShoppingApp.Data.Context;
using ShoppingApp.Data.Entities;

public class UserService : IUserService
{
    private readonly ShoppingAppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(ShoppingAppDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    // Kullanıcı oluşturma (şifre hash'leme işlemi)
    public async Task<User> CreateUserAsync(User user, string password)
    {
        // Şifreyi hash'liyoruz
        user.Password = _passwordHasher.HashPassword(password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    // Kullanıcıyı ID'ye göre bulma
    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    // Kullanıcı bilgilerini güncelleme (şifre değişikliği de yapılabilir)
    public async Task UpdateUserAsync(User user, int currentUserId, string newPassword = null)
    {
        // ID değiştirilmemeli
        if (user.Id != currentUserId)
        {
            throw new UnauthorizedAccessException("Sadece kendi hesabınızı güncelleyebilirsiniz.");
        }

        // Kullanıcı rolü değiştirilmemeli
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        if (existingUser == null)
        {
            throw new KeyNotFoundException("Kullanıcı bulunamadı.");
        }

        if (user.Role != existingUser.Role)
        {
            throw new UnauthorizedAccessException("Rol değiştirilemez.");
        }

        // Şifre varsa, hash'leyip güncelliyoruz
        if (!string.IsNullOrEmpty(newPassword))
        {
            user.Password = _passwordHasher.HashPassword(newPassword);
        }

        // Güncellenen bilgileri mevcut kullanıcıyla birleştiriyoruz
        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Email = user.Email;
        existingUser.PhoneNumber = user.PhoneNumber;
        existingUser.Role = user.Role; // Role değiştirilemiyor, sadece mevcut role uygun olmalı

        // Veritabanında güncelleniyor
        _context.Users.Update(existingUser);
        await _context.SaveChangesAsync();
    }

    // Kullanıcıyı silme
    public async Task DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("Kullanıcı bulunamadı.");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    // Kullanıcı adı ve şifre ile kullanıcıyı doğrulama (login)
    public async Task<User> AuthenticateAsync(string email, string password)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

        if (user == null || !_passwordHasher.VerifyPassword(user.Password, password))
            return null;

        return user;
    }

    // Kullanıcı giriş yaparken kullanılan işlem
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
