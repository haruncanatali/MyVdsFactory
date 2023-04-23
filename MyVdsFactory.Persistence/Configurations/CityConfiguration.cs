using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyVdsFactory.Domain.Entities;
using MyVdsFactory.Domain.Enums;

namespace MyVdsFactory.Persistence.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasQueryFilter(c => c.Status == EntityStatus.Active);

        builder.Property(c => c.Name).IsRequired();
        builder.Property(c => c.Latitude).HasDefaultValue(0).IsRequired();
        builder.Property(c => c.Longitude).HasDefaultValue(0).IsRequired();
        builder.Property(c => c.Population).HasDefaultValue(0).IsRequired();

        builder.HasMany(c => c.Districts)
            .WithOne(c => c.City)
            .HasForeignKey(c => c.CityId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(c => c.Prayers)
            .WithOne(c => c.City)
            .HasForeignKey(c => c.CityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}