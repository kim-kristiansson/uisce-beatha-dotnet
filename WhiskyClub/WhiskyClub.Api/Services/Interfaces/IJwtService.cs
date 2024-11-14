using WhiskyClub.Api.Models;

namespace WhiskyClub.Api.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
