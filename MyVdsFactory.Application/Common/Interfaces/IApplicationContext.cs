using Microsoft.EntityFrameworkCore;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Common.Interfaces;

public interface IApplicationContext
{
    public DbSet<User> Users{ get; set; }
    public DbSet<Role> Roles{ get; set; }
    public DbSet<UserRole> UserRoles{ get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}