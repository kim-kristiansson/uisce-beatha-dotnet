using UisceBeatha.Api.Models;

namespace UisceBeatha.Api.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
