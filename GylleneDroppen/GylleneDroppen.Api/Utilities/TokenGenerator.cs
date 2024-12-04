using System.Security.Cryptography;

namespace GylleneDroppen.Api.Utilities;

public static class TokenGenerator
{
    public static string GenerateToken()
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(tokenBytes);
    }
}