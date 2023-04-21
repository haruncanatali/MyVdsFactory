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
    public DbSet<Terror> Terrors { get; set; }
    public DbSet<Horoscope> Horoscopes { get; set; }
    public DbSet<HoroscopeCommentary> HoroscopeCommentaries { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Prayer> Prayers { get; set; }
    public DbSet<Tarot> Tarots { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}