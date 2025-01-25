using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Dtos
{
    // Kullanıcı girişi için kullanılan DTO (Data Transfer Object)
    public class LoginUserDto
    {
        [Required] // Bu alanın doldurulması zorunludur.
        [EmailAddress] // E-posta adresinin geçerli bir formatta olması gereklidir.
        public string Email { get; set; } // Kullanıcının e-posta adresi.

        [Required] // Bu alanın doldurulması zorunludur.
        public string Password { get; set; } // Kullanıcının şifresi.
    }
}
