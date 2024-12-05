using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GylleneDroppen.Api.Configurations;
using GylleneDroppen.Api.Models;
using GylleneDroppen.Api.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace GylleneDroppen.Api.Services
{
    public class JwtService :IJwtService
    {
        private readonly JwtConfig _jwtConfig;
        private readonly byte[] _key;

        public JwtService(IOptions<JwtConfig> jwtConfigOptions)
        {
            _jwtConfig = jwtConfigOptions.Value;
            _key = System.Text.Encoding.UTF8.GetBytes(_jwtConfig.SecretKey ?? throw new InvalidOperationException("SecretKey is required."));
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
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
