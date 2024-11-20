using Microsoft.EntityFrameworkCore;
using UisceBeatha.Api.Models;

namespace UisceBeatha.Api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) :DbContext(options)
    {
        public DbSet<User>? Users { get; init; }
        public DbSet<EmailOfInterest>? Emails { get; init; }
    }
}
