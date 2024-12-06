using System.Text;
using GylleneDroppen.Api.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace GylleneDroppen.Api.Extensions;

public static class AuthenticationServiceExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfig = configuration.GetSection("JwtConfig").Get<JwtConfig>();
        
        if (jwtConfig == null)
        {
            throw new ArgumentException("JWT configuration is missing. Make sure it is set in the appsettings or environment variables.");
        }

        if (string.IsNullOrWhiteSpace(jwtConfig.SecretKey))
        {
            throw new ArgumentException("JWT SecretKey is not configured. Ensure it is set as an environment variable or in the configuration.");
        }

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = !string.IsNullOrWhiteSpace(jwtConfig.Issuer),
                    ValidateAudience = !string.IsNullOrWhiteSpace(jwtConfig.Audience),
                    ValidateLifetime = true,
                };
            });

        return services;
    }
}