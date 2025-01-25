using ShoppingApp.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Services
{
    // Parola hashleme ve doğrulama işlemlerini sağlayan sınıf
    public class PasswordHasher : IPasswordHasher
    {
        // Girilen parolayı SHA256 algoritması kullanarak hashler ve Base64 formatında döndürür.
        public string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }

        // Hashlenmiş parola ile girilen parolanın eşleşip eşleşmediğini kontrol eder.
        public bool VerifyPassword(string hashedPassword, string password)
        {
            // Girilen parolayı hashleyip, hashlenmiş parola ile karşılaştırır.
            var hashedPasswordInput = HashPassword(password);
            return hashedPasswordInput == hashedPassword;
        }
    }
}
