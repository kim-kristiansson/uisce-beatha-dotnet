using GylleneDroppen.Api.Services.Interfaces;

namespace GylleneDroppen.Api.Services
{
    public class PasswordService :IPasswordService
    {
        public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
        public bool VerifyPassword(string password, string passwordHash) => BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
