using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Persistence;

public class ApplicationContext : IdentityDbContext<User, Role, long, IdentityUserClaim<long>,
        UserRole, IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>>,
        IApplicationContext
    {
        private readonly ICurrentUserService _currentUserService;

        public ApplicationContext(DbContextOptions<ApplicationContext> options, ICurrentUserService currentUserService)
            : base(options)
        {
            _currentUserService = currentUserService;
        }
        
        #region Identity User Tables

        public DbSet<User> Users
        {
            get { return base.Users; }
            set { }
        }

        public DbSet<Role> Roles
        {
            get { return base.Roles; }
            set { }
        }

        public DbSet<UserRole> UserRoles
        {
            get { return base.UserRoles; }
            set { }
        }

        public DbSet<Earthquake> Earthquakes { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Terror> Terrors { get; set; }
        public DbSet<Horoscope> Horoscopes { get; set; }
        public DbSet<HoroscopeCommentary> HoroscopeCommentaries { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Prayer> Prayers { get; set; }
        public DbSet<Tarot> Tarots { get; set; }

        #endregion


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.CreatedAt = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = _currentUserService.UserId;
                        entry.Entity.UpdatedAt = DateTime.Now;
                        break;
                    case EntityState.Deleted:
                        entry.Entity.DeletedBy = _currentUserService.UserId;
                        entry.Entity.DeletedAt = DateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }