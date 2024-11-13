using Microsoft.EntityFrameworkCore;
using WhiskyClub.Api.Data;
using WhiskyClub.Api.Models;
using WhiskyClub.Api.Repositories.Interfaces;

namespace WhiskyClub.Api.Repositories
{
    public class UserRepository(AppDbContext context) :Repository<User>(context), IUserRepository
    {
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> IsEmailRegisteredAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }
    }
}
