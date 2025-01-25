using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingApp.Data.Entities
{
    // Kullanıcı tablosunu temsil eden varlık sınıfı
    public class User
    {
        [Key] // Birincil anahtar tanımı
        public int Id { get; set; } // Kullanıcı ID'si

        [MaxLength(50)] // Ad alanı için maksimum uzunluk 50 karakter
        public string FirstName { get; set; } // Kullanıcının adı

        [MaxLength(50)] // Soyad alanı için maksimum uzunluk 50 karakter
        public string LastName { get; set; } // Kullanıcının soyadı

        [EmailAddress] // E-posta adresi formatının doğrulanması
        public string Email { get; set; } // Kullanıcının e-posta adresi

        [Phone] // Telefon numarası formatının doğrulanması
        public string PhoneNumber { get; set; } // Kullanıcının telefon numarası

        public string Password { get; set; } // Kullanıcının şifresi (hashlenmiş olarak saklanır)

        public Role Role { get; set; } // Kullanıcının rolü (ör. Admin, Customer)
    }
}
