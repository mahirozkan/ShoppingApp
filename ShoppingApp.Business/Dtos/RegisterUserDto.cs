using ShoppingApp.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Business.Dtos
{
    // Yeni bir kullanıcı kaydetmek için kullanılan DTO (Data Transfer Object)
    public class RegisterUserDto
    {
        [Required] // Ad alanı zorunludur.
        [MaxLength(50)] // Ad alanı maksimum 50 karakter uzunluğunda olabilir.
        public string FirstName { get; set; } // Kullanıcının adı.

        [Required] // Soyad alanı zorunludur.
        [MaxLength(50)] // Soyad alanı maksimum 50 karakter uzunluğunda olabilir.
        public string LastName { get; set; } // Kullanıcının soyadı.

        [Required] // Email alanı zorunludur.
        [EmailAddress] // Email alanının geçerli bir e-posta formatında olması zorunludur.
        public string Email { get; set; } // Kullanıcının e-posta adresi.

        [Required] // Telefon numarası alanı zorunludur.
        public string PhoneNumber { get; set; } // Kullanıcının telefon numarası.

        [Required] // Şifre alanı zorunludur.
        public string Password { get; set; } // Kullanıcının şifresi.

        [Required] // Role alanı zorunludur.
        public Role Role { get; set; } // Kullanıcının rolü (yetki seviyesi).
    }
}
