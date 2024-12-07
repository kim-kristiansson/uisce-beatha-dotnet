using GylleneDroppen.Api.Configurations;
using GylleneDroppen.Api.Repositories;
using GylleneDroppen.Api.Repositories.Interfaces;
using GylleneDroppen.Api.Services;
using GylleneDroppen.Api.Services.Interfaces;

namespace GylleneDroppen.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetSection("ConnectionStrings").Get<ConnectionStringsConfig>()
            ?.RedisConnection;

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
        });
        
        return services;
    }
    
    public static IServiceCollection AddAppConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));
        services.Configure<SmtpConfig>(configuration.GetSection("SmtpConfig"));
        services.Configure<NewsletterConfig>(configuration.GetSection("NewsletterConfig"));
        services.Configure<EmailAccountsConfig>(configuration.GetSection("EmailAccountsConfig"));
        services.Configure<StripeConfig>(configuration.GetSection("StripeConfig"));
        services.Configure<ConnectionStringsConfig>(configuration.GetSection("ConnectionStrings"));
        services.Configure<GlobalConfig>(configuration.GetSection("GlobalConfig"));
        services.Configure<FrontendConfig>(configuration.GetSection("FrontendConfig"));

        return services;
    }
    
    public static IServiceCollection AddScopedServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IStripeService, StripeService>();
        services.AddScoped<ISmtpService, SmtpService>();
        services.AddScoped<INewsletterService, NewsletterService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();
        services.AddScoped<IJwtService, JwtService>();
        return services;
    }
    
    public static IServiceCollection AddScopedRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<INewsletterRepository, NewsletterRepository>();
        services.AddScoped<IRedisRepository, RedisRepository>();
        services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();
        return services;
    }
}