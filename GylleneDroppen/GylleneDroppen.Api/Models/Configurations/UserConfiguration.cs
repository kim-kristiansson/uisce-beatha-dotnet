using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GylleneDroppen.Api.Models.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Email).HasMaxLength(320);
        builder.Property(u => u.Firstname).HasMaxLength(100);
        builder.Property(u => u.Lastname).HasMaxLength(100);
        builder.Property(u => u.PasswordHash).HasMaxLength(255);
        
        builder
            .HasOne(u => u.Address)
            .WithOne(a => a.User)
            .HasForeignKey<Address>(a => a.UserId);
    }
}