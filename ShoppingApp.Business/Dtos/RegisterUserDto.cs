using ShoppingApp.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Business.Dtos
{
    public class RegisterUserDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public Role Role { get; set; }
    }
}
