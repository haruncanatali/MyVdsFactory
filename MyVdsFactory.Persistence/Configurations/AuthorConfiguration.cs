using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyVdsFactory.Domain.Entities;
using MyVdsFactory.Domain.Enums;

namespace MyVdsFactory.Persistence.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasQueryFilter(c => c.Status == EntityStatus.Active);

        builder.Property(c => c.FullName).IsRequired();

        builder.HasMany(c => c.Books)
            .WithOne(c => c.Author)
            .HasForeignKey(c => c.AurhorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}