using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GylleneDroppen.Api.Models.Configurations;

public class BillingInfoConfiguration : IEntityTypeConfiguration<BillingInfo>
{
    public void Configure(EntityTypeBuilder<BillingInfo> builder)
    {
           builder.Property(b => b.StreetAddress)
                  .HasMaxLength(255);

           builder.Property(b => b.City)
                  .HasMaxLength(100);

           builder.Property(b => b.PostalCode)
                  .HasMaxLength(20);

           builder.Property(b => b.Country)
                  .HasMaxLength(100)
                  .HasDefaultValue("Sweden");

           builder.HasOne<User>()
                  .WithOne(u => u.BillingInfo)
                  .HasForeignKey<BillingInfo>(b => b.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
    }
}