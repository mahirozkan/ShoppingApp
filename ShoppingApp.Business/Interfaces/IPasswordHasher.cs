using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Interfaces
{
    // Parola hashleme ve doğrulama işlemleri için kullanılan arayüz
    public interface IPasswordHasher
    {
        string HashPassword(string password); // Gönderilen parolayı hashler ve hash değerini döner.

        bool VerifyPassword(string hashedPassword, string password); // Hashlenmiş parola ile verilen parolayı karşılaştırır ve doğruluk sonucunu döner.
    }
}

