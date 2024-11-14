﻿namespace WhiskyClub.Api.Services.Configurations
{
    public class JwtSettings
    {
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? SecurityKey { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}
