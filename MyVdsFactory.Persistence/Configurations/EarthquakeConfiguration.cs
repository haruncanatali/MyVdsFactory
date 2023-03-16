using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyVdsFactory.Domain.Entities;
using MyVdsFactory.Domain.Enums;

namespace MyVdsFactory.Persistence.Configurations
{
    internal class EarthquakeConfiguration : IEntityTypeConfiguration<Earthquake>
    {
        public void Configure(EntityTypeBuilder<Earthquake> builder)
        {
            builder.Property(c => c.Rms).IsRequired();
            builder.Property(c => c.Latitude).IsRequired();
            builder.Property(c => c.Longitude).IsRequired();
            builder.Property(c=>c.Magnitude).IsRequired();
            builder.Property(c => c.Location).IsRequired();
            builder.Property(c => c.Country).IsRequired();
            builder.Property(c => c.Province).IsRequired();
            builder.Property(c => c.District).IsRequired();
            builder.Property(c => c.Date).IsRequired();
            builder.Property(c => c.Year).HasDefaultValue(2006).IsRequired();
            builder.Property(c => c.Month).HasDefaultValue(1).IsRequired();
            builder.Property(c => c.Day).HasDefaultValue(1).IsRequired();

            builder.HasQueryFilter(c => c.Status == EntityStatus.Active);
        }
    }
}