using ShoppingApp.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Business.Dtos
{
    public class UserUpdateDto
    {
        [Required, MaxLength(50)] // FirstName alanı zorunlu ve maksimum 50 karakter uzunluğunda olmalıdır.
        public string FirstName { get; set; }

        [Required, MaxLength(50)] // LastName alanı zorunlu ve maksimum 50 karakter uzunluğunda olmalıdır.
        public string LastName { get; set; }

        [Required, EmailAddress] // Email alanı zorunlu ve geçerli bir e-posta adresi formatında olmalıdır.
        public string Email { get; set; }

        [Required, Phone] // PhoneNumber alanı zorunlu ve geçerli bir telefon numarası formatında olmalıdır.
        public string PhoneNumber { get; set; }

        // Password alanı isteğe bağlıdır ve herhangi bir doğrulama kısıtlaması uygulanmamıştır.
        public string Password { get; set; }
    }
}
