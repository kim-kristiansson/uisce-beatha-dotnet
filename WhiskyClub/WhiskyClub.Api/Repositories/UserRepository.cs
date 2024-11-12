using WhiskyClub.Api.Data;
using WhiskyClub.Api.Models;

namespace WhiskyClub.Api.Repositories.Interfaces
{
    public class UserRepository(AppDbContext context) :Repository<User>(context), IUserRepository
    {
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
