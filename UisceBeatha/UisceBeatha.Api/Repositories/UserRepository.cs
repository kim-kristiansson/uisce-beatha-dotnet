using Microsoft.EntityFrameworkCore;
using UisceBeatha.Api.Data;
using UisceBeatha.Api.Models;
using UisceBeatha.Api.Repositories.Interfaces;

namespace UisceBeatha.Api.Repositories
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
