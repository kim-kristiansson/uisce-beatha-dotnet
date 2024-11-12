using Microsoft.EntityFrameworkCore;
using WhiskyClub.Api.Models;

namespace WhiskyClub.Api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) :DbContext(options)
    {
        public DbSet<User>? Users { get; set; }
    }
}
