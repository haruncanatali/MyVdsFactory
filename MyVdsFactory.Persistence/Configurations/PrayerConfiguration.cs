using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyVdsFactory.Domain.Entities;
using MyVdsFactory.Domain.Enums;

namespace MyVdsFactory.Persistence.Configurations;

public class PrayerConfiguration : IEntityTypeConfiguration<Prayer>
{
    public void Configure(EntityTypeBuilder<Prayer> builder)
    {
        builder.HasQueryFilter(c => c.Status == EntityStatus.Active);

        builder.Property(c => c.Date).IsRequired();
        builder.Property(c => c.Fajr).IsRequired();
        builder.Property(c => c.Tulu).IsRequired();
        builder.Property(c => c.Zuhr).IsRequired();
        builder.Property(c => c.Asr).IsRequired();
        builder.Property(c => c.Maghrib).IsRequired();
        builder.Property(c => c.Isha).IsRequired();
    }
}