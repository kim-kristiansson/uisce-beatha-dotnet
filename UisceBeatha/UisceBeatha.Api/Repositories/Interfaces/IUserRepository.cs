using UisceBeatha.Api.Models;

namespace UisceBeatha.Api.Repositories.Interfaces
{
    public interface IUserRepository :IRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> IsEmailRegisteredAsync(string email);
    }
}
