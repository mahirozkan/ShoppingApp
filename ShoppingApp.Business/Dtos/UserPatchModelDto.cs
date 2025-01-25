using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Dtos
{
    // Kullanıcı bilgilerini güncellerken sadece belirtilen alanların yama (patch) şeklinde gönderilmesini sağlar.
    public class UserPatchModelDto
    {
        public string FirstName { get; set; } // Kullanıcının ad bilgisi (isteğe bağlı).

        public string LastName { get; set; } // Kullanıcının soyad bilgisi (isteğe bağlı).

        [EmailAddress] // E-posta adresinin formatının geçerli olmasını zorunlu kılar.
        public string Email { get; set; } // Kullanıcının e-posta adresi (isteğe bağlı).

        [Phone] // Telefon numarasının formatının geçerli olmasını zorunlu kılar.
        public string PhoneNumber { get; set; } // Kullanıcının telefon numarası (isteğe bağlı).
    }
}
