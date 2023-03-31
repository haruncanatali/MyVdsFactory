using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyVdsFactory.Domain.Entities;
using MyVdsFactory.Domain.Enums;

namespace MyVdsFactory.Persistence.Configurations;

public class HoroscopeConfiguration : IEntityTypeConfiguration<Horoscope>
{
    public void Configure(EntityTypeBuilder<Horoscope> builder)
    {
        builder.HasQueryFilter(c => c.Status == EntityStatus.Active);

        builder.Property(c => c.Name).IsRequired();
        builder.Property(c => c.PhotoName).IsRequired();
        builder.Property(c => c.DateRange).IsRequired();
        builder.Property(c => c.NormalizedName).IsRequired();
        builder.Property(c => c.Group).IsRequired();
        builder.Property(c => c.Description).IsRequired();
        builder.Property(c => c.Planet).IsRequired();

        builder
            .HasMany(c => c.HoroscopeCommentaries)
            .WithOne(c => c.Horoscope)
            .HasForeignKey(c => c.HoroscopeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}