using WhiskyClub.Api.Models;

namespace WhiskyClub.Api.Repositories.Interfaces
{
    public interface IUserRepository :IRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> IsEmailRegisteredAsync(string email);
    }
}
