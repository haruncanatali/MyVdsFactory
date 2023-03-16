using Microsoft.EntityFrameworkCore;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Common.Interfaces;

public interface IApplicationContext
{
    public DbSet<User> Users{ get; set; }
    public DbSet<Role> Roles{ get; set; }
    public DbSet<UserRole> UserRoles{ get; set; }
    public DbSet<Earthquake> Earthquakes { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<District> Districts { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}