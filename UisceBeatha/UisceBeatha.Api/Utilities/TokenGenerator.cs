using System.Security.Cryptography;

namespace UisceBeatha.Api.Utilities;

public static class TokenGenerator
{
    public static string GenerateToken()
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(tokenBytes);
    }
}