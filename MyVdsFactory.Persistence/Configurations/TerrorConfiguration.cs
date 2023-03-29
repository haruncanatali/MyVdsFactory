using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Persistence.Configurations;

public class TerrorConfiguration : IEntityTypeConfiguration<Terror>
{
    public void Configure(EntityTypeBuilder<Terror> builder)
    {
        builder.Property(c => c.Year).HasDefaultValue(2000).IsRequired();
        builder.Property(c => c.Month).HasDefaultValue(1).IsRequired();
        builder.Property(c => c.Day).HasDefaultValue(1).IsRequired();
        builder.Property(c => c.Date).IsRequired();
        builder.Property(c => c.Country).HasDefaultValue("Bilinmeyen").IsRequired();
        builder.Property(c => c.Region).HasDefaultValue("Bilinmeyen").IsRequired();
        builder.Property(c => c.City).HasDefaultValue("Bilinmeyen").IsRequired();
        builder.Property(c => c.Location).HasDefaultValue("Bilinmeyen").IsRequired();
        builder.Property(c => c.Longitude).HasDefaultValue(0.0).IsRequired();
        builder.Property(c => c.Latitude).HasDefaultValue(0.0).IsRequired();
        builder.Property(c => c.Summary).HasDefaultValue("Eklenmemiş").IsRequired();
        builder.Property(c => c.Alternative).HasDefaultValue("Eklenmemiş").IsRequired();
        builder.Property(c => c.Success).HasDefaultValue(true).IsRequired();
        builder.Property(c => c.Suicide).HasDefaultValue(true).IsRequired();
        builder.Property(c => c.AttackType).HasDefaultValue("Eklenmemiş").IsRequired();
        builder.Property(c => c.TargetType).HasDefaultValue("Eklenmemiş").IsRequired();
        builder.Property(c => c.TargetSubType).HasDefaultValue("Eklenmemiş").IsRequired();
        builder.Property(c => c.GroupName).HasDefaultValue("Bilinmeyen").IsRequired();
        builder.Property(c => c.GroupSubName).HasDefaultValue("Bilinmeyen").IsRequired();
        builder.Property(c => c.WeaponType).HasDefaultValue("Bilinmeyen").IsRequired();
        builder.Property(c => c.WeaponSubType).HasDefaultValue("Bilinmeyen").IsRequired();
        builder.Property(c => c.WeaponDetail).HasDefaultValue("Bilinmeyen").IsRequired();
        builder.Property(c => c.Kill).HasDefaultValue(0).IsRequired();
        builder.Property(c => c.DbSource).HasDefaultValue("Bilinmeyen").IsRequired();
        builder.Property(c => c.CityLatitude).HasDefaultValue(0.0).IsRequired();
        builder.Property(c => c.CityLongitude).HasDefaultValue(0.0).IsRequired();
        builder.Property(c => c.CountryLatitude).HasDefaultValue(0.0).IsRequired();
        builder.Property(c => c.CountryLongitude).HasDefaultValue(0.0).IsRequired();
    }
}