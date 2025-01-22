using Microsoft.IdentityModel.Tokens;
using ShoppingApp.WebApi.Jwt;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShoppingApp.WebApi.Jwt
{
    public static class JwtHelper
    {
        public static string GenerateJwtToken(JwtDto jwtInfo)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtInfo.SecretKey));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtClaimNames.Id, jwtInfo.Id.ToString()),
                new Claim(JwtClaimNames.Email, jwtInfo.Email),
                new Claim(JwtClaimNames.FirstName, jwtInfo.FirstName),
                new Claim(JwtClaimNames.LastName, jwtInfo.LastName),
                new Claim(JwtClaimNames.Role, jwtInfo.Role.ToString()),
                new Claim(ClaimTypes.Role, jwtInfo.Role.ToString())
            };

            var expireTime = DateTime.UtcNow.AddMinutes(jwtInfo.ExpiryInMinutes);

            var tokenDescriptor = new JwtSecurityToken(
                jwtInfo.Issuer,
                jwtInfo.Audience,
                claims,
                expires: expireTime,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
