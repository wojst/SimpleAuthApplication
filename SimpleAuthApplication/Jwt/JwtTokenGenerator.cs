using Microsoft.IdentityModel.Tokens;
using SimpleAuthApplication.Dtos;
using SimpleAuthApplication.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleAuthApplication.Jwt
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly string _jwtSecret;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _jwtExpiryMinutes;

        public JwtTokenGenerator(string jwtSecret, string issuer, string audience, int jwtExpiryMinutes)
        {
            _jwtSecret = jwtSecret;
            _issuer = issuer;
            _audience = audience;
            _jwtExpiryMinutes = jwtExpiryMinutes;
        }

        public TokenDto GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Auth.Login),
                new Claim(ClaimTypes.Email, user.Auth.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtExpiryMinutes),
                signingCredentials: creds
                );

            return new TokenDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = GenerateRefreshToken(user.Id)
            };
        }

        private string GenerateRefreshToken(int userId)
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}
