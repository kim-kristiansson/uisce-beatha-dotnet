using GylleneDroppen.Api.Models;
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
            modelBuilder.Entity<NewsletterSubscription>()
                .Property(n => n.Email)
                .HasMaxLength(320);
        }
    }
}
