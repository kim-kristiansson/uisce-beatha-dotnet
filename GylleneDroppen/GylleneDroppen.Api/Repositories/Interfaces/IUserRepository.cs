using GylleneDroppen.Api.Models;

namespace GylleneDroppen.Api.Repositories.Interfaces
{
    public interface IUserRepository :IRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> IsEmailRegisteredAsync(string email);
    }
}
