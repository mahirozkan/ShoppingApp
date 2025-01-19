using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Dtos
{
    public class LoginRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
