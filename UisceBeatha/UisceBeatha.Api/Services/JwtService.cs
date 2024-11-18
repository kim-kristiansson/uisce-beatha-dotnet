using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UisceBeatha.Api.Configurations;
using UisceBeatha.Api.Models;
using UisceBeatha.Api.Services.Interfaces;

namespace UisceBeatha.Api.Services
{
    public class JwtService :IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly byte[] _key;

        public JwtService(IConfiguration configuration)
        {
            _jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? throw new ArgumentException("JwtSettings not configured");

            string securityKey = _jwtSettings.SecurityKey ?? throw new ArgumentNullException("SecurityKey not configured in JwtSettings", nameof(_jwtSettings.SecurityKey));

            _key = System.Text.Encoding.UTF8.GetBytes(securityKey);

        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
