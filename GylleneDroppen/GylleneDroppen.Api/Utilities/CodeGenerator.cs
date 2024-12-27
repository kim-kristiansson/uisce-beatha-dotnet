namespace GylleneDroppen.Api.Utilities;

public static class CodeGenerator
{
    public static string GenerateVerificationCode()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}