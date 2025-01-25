using ShoppingApp.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Business.Dtos
{
    // Kullanıcının temel bilgilerini temsil eden DTO (Data Transfer Object)
    public class UserInfoDto
    {
        [Required] // Id alanı zorunludur.
        public int Id { get; set; } // Kullanıcının benzersiz kimlik numarası.

        [Required]
        [EmailAddress] // Email alanı geçerli bir e-posta formatında olmalıdır.
        public string Email { get; set; } // Kullanıcının e-posta adresi.

        [Required] // FirstName alanı zorunludur.
        public string FirstName { get; set; } // Kullanıcının adı.

        [Required] // LastName alanı zorunludur.
        public string LastName { get; set; } // Kullanıcının soyadı.

        [Required] // Role alanı zorunludur.
        public Role Role { get; set; } // Kullanıcının rolü (yetki seviyesi).
    }
}
