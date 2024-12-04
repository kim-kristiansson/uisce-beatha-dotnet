using GylleneDroppen.Api.Data;
using GylleneDroppen.Api.Models;
using GylleneDroppen.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Api.Repositories
{
    public class UserRepository(AppDbContext context) :Repository<User>(context), IUserRepository
    {
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await DbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> IsEmailRegisteredAsync(string email)
        {
            return await DbSet.AnyAsync(u => u.Email == email);
        }
    }
}
