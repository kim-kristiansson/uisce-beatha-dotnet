using GylleneDroppen.Api.Models;

namespace GylleneDroppen.Api.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
