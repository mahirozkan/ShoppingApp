using ShoppingApp.Data.Entities;

namespace ShoppingApp.WebApi.Jwt
{
    // JWT (JSON Web Token) için gerekli bilgileri tutan Data Transfer Object (DTO) sınıfı
    public class JwtDto
    {
        // Kullanıcı kimliği (token'a dahil edilecek)
        public int Id { get; set; }

        // Kullanıcı e-posta adresi (token'a dahil edilecek)
        public string Email { get; set; }

        // Kullanıcının adı (token'a dahil edilecek)
        public string FirstName { get; set; }

        // Kullanıcının soyadı (token'a dahil edilecek)
        public string LastName { get; set; }

        // Kullanıcı rolü (token'a dahil edilecek)
        public Role Role { get; set; }

        // JWT oluşturmak için kullanılacak gizli anahtar
        public string SecretKey { get; set; }

        // JWT sağlayıcısının (issuer) bilgisi
        public string Issuer { get; set; }

        // JWT'nin hangi hedef kitleye (audience) yönelik olduğunu belirtir
        public string Audience { get; set; }

        // JWT'nin geçerlilik süresi (dakika olarak)
        public int ExpiryInMinutes { get; set; }
    }
}
