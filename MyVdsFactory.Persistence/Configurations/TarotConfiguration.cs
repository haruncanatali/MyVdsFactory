using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyVdsFactory.Domain.Entities;
using MyVdsFactory.Domain.Enums;

namespace MyVdsFactory.Persistence.Configurations;

public class TarotConfiguration : IEntityTypeConfiguration<Tarot>
{
    public void Configure(EntityTypeBuilder<Tarot> builder)
    {
        builder.HasQueryFilter(c => c.Status == EntityStatus.Active);

        builder.Property(c => c.Name).IsRequired();
        builder.Property(c => c.Description).IsRequired();
        builder.Property(c => c.Features).IsRequired();
        builder.Property(c => c.PhotoUrl).IsRequired();
    }
}