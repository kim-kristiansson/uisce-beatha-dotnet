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
        var configMappings = new Dictionary<Type, string>
        {
            { typeof(JwtConfig), "JwtConfig" },
            { typeof(SmtpConfig), "SmtpConfig" },
            { typeof(NewsletterConfig), "NewsletterConfig" },
            { typeof(EmailAccountsConfig), "EmailAccountsConfig" },
            { typeof(StripeConfig), "StripeConfig" },
            { typeof(ConnectionStringsConfig), "ConnectionStrings" },
            { typeof(GlobalConfig), "GlobalConfig" },
            { typeof(FrontendConfig), "FrontendConfig" }
        };

        foreach (var (configType, sectionName) in configMappings)
        {
            var method = typeof(OptionsConfigurationServiceCollectionExtensions)
                .GetMethod("Configure", [typeof(IServiceCollection), typeof(IConfigurationSection)])!
                .MakeGenericMethod(configType);

            method.Invoke(null, [services, configuration.GetSection(sectionName)]);
        }

        return services;
    }
    public static IServiceCollection AddScopedServices(this IServiceCollection services)
    {
        var serviceMappings = new Dictionary<Type, Type>
        {
            { typeof(IAuthService), typeof(AuthService) },
            { typeof(IStripeService), typeof(StripeService) },
            { typeof(ISmtpService), typeof(SmtpService) },
            { typeof(INewsletterService), typeof(NewsletterService) },
            { typeof(IAnalyticsService), typeof(AnalyticsService) },
            { typeof(IJwtService), typeof(JwtService) }
        };

        foreach (var mapping in serviceMappings)
        {
            services.AddScoped(mapping.Key, mapping.Value);
        }

        return services;
    }
    
    public static IServiceCollection AddScopedRepositories(this IServiceCollection services)
    {
        var repositoryMappings = new Dictionary<Type, Type>
        {
            { typeof(IUserRepository), typeof(UserRepository) },
            { typeof(INewsletterRepository), typeof(NewsletterRepository) },
            { typeof(IRedisRepository), typeof(RedisRepository) },
            { typeof(IAnalyticsRepository), typeof(AnalyticsRepository) }
        };

        foreach (var mapping in repositoryMappings)
        {
            services.AddScoped(mapping.Key, mapping.Value);
        }

        return services;
    }
}