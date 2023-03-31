using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyVdsFactory.Domain.Entities;
using MyVdsFactory.Domain.Enums;

namespace MyVdsFactory.Persistence.Configurations;

public class HoroscopeCommentaryConfiguration : IEntityTypeConfiguration<HoroscopeCommentary>
{
    public void Configure(EntityTypeBuilder<HoroscopeCommentary> builder)
    {
        builder.HasQueryFilter(c => c.Status == EntityStatus.Active);

        builder.Property(c => c.Commentary).IsRequired();
        builder.Property(c => c.Date).IsRequired();
        builder.Property(c => c.HoroscopeId).IsRequired();
    }
}