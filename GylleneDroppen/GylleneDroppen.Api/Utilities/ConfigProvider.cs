using GylleneDroppen.Api.Utilities.Interfaces;

namespace GylleneDroppen.Api.Utilities;

public class ConfigProvider<T> : IConfigProvider<T> where T : class, new()
{
    private readonly T _config;

    public ConfigProvider(IConfiguration configuration, string sectionName)
    {
        _config = configuration.GetSection(sectionName).Get<T>() 
                  ?? throw new ArgumentException($"Configuration section '{sectionName}' is missing or invalid.");
        
        ValidateConfig(_config, sectionName);
    }

    public T GetConfig() => _config;
    
    private static void ValidateConfig(T config, string sectionName)
    {
        var properties = typeof(T).GetProperties()
            .Where(prop => prop.GetIndexParameters().Length == 0);

        foreach (var property in properties)
        {
            var value = property.GetValue(config);
            if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
            {
                throw new ArgumentException($"Property '{property.Name}' in section '{sectionName}' is not configured properly.");
            }
        }
    }
}