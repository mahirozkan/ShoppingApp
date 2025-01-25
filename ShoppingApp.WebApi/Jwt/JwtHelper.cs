using Microsoft.IdentityModel.Tokens;
using ShoppingApp.WebApi.Jwt;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShoppingApp.WebApi.Jwt
{
    // JWT oluşturmak için yardımcı metotlar sağlayan sınıf
    public static class JwtHelper
    {
        // JWT token üretmek için kullanılan metot
        public static string GenerateJwtToken(JwtDto jwtInfo)
        {
            // Secret key'i simetrik bir güvenlik anahtarına dönüştürür
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtInfo.SecretKey));

            // HMAC SHA256 algoritması kullanılarak imzalama bilgilerini oluşturur
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            // Token'a eklenecek claim'leri (kullanıcıya dair verileri) tanımlar
            var claims = new[]
            {
                new Claim(JwtClaimNames.Id, jwtInfo.Id.ToString()), // Kullanıcı ID
                new Claim(JwtClaimNames.Email, jwtInfo.Email), // Kullanıcı email
                new Claim(JwtClaimNames.FirstName, jwtInfo.FirstName), // Kullanıcı adı
                new Claim(JwtClaimNames.LastName, jwtInfo.LastName), // Kullanıcı soyadı
                new Claim(JwtClaimNames.Role, jwtInfo.Role.ToString()), // Uygulama özelinde kullanıcı rolü
                new Claim(ClaimTypes.Role, jwtInfo.Role.ToString()) // Standart kullanıcı rolü
            };

            // Token'ın sona erme süresi UTC saatine göre ayarlanır
            var expireTime = DateTime.UtcNow.AddMinutes(jwtInfo.ExpiryInMinutes);

            // Token açıklamasını (descriptor) oluşturur
            var tokenDescriptor = new JwtSecurityToken(
                jwtInfo.Issuer, // Token'ı sağlayan sistem
                jwtInfo.Audience, // Token'ın hedef kitlesi
                claims, // Token'a eklenecek veriler (claim'ler)
                expires: expireTime, // Token geçerlilik süresi
                signingCredentials: credentials // İmzalama bilgileri
            );

            // Token'ı string formatında döndürür
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
