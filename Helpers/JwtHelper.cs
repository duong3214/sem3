using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backendMuseum.Models;
using Microsoft.IdentityModel.Tokens;

namespace backendMuseum.Helpers
{
    public class JwtHelper
    {
        private readonly string _secretKey;

        public JwtHelper(string secretKey)
        {
            _secretKey = secretKey ?? throw new ArgumentNullException(nameof(secretKey));
        }

        public string GenerateToken(User user)
        {
            // Kiểm tra nếu user.UserName hoặc user.Email là null hoặc empty, bạn có thể đưa ra giá trị mặc định hoặc ném lỗi.
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentException("UserName and Email cannot be null or empty.");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
