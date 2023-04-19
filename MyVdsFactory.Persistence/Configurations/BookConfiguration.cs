using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyVdsFactory.Domain.Entities;
using MyVdsFactory.Domain.Enums;

namespace MyVdsFactory.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasQueryFilter(c => c.Status == EntityStatus.Active);

        builder.Property(c => c.Name).IsRequired();
    }
}