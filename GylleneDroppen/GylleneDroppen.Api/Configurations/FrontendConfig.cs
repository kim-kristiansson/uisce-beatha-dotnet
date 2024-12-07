namespace GylleneDroppen.Api.Configurations;

public class FrontendConfig
{
    public string BaseUrl { get; init; } = string.Empty;
    public PathConfig Paths { get; init; } = new();
}