using GylleneDroppen.Api.Models;
using GylleneDroppen.Api.Models.Configurations;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) :DbContext(options)
    {
        public DbSet<User> Users { get; init; }
        public DbSet<NewsletterSubscription> NewsletterSubscriptions { get; init; }
        public DbSet<Analytics> Analytics { get; init; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new NewsletterSubscriptionConfiguration());
        }
    }
}
