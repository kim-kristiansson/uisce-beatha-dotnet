using System.Security.Cryptography;

namespace GylleneDroppen.Api.Utilities;

public class CodeGenerator
{
    public static string GenerateVerificationCode()
    {
        var randomNumberGenerator = RandomNumberGenerator.Create();
        
        var buffer = new byte[4];
        randomNumberGenerator.GetBytes(buffer);
        
        var randomNumber = BitConverter.ToUInt32(buffer, 0) % 900000 + 100000;
        
        return randomNumber.ToString();
    }
}