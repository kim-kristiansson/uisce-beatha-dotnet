namespace UisceBeatha.Api.Configurations
{
    public class JwtSettings
    {
        public string? Issuer { get; init; }
        public string? Audience { get; init; }
        public string? SecurityKey { get; init; }
        public int ExpiryMinutes { get; init; }
    }
}
