using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyVdsFactory.Domain.Entities;
using MyVdsFactory.Domain.Enums;

namespace MyVdsFactory.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasQueryFilter(c => c.Status == EntityStatus.Active);
    }
}