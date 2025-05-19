using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using PunchSystem.Services;
using PunchSystem.Models;

namespace PunchSystem.Data
{
    public class AppDbContext:DbContext
    {
        private readonly IUserContextService _userContext;
        public AppDbContext(DbContextOptions<AppDbContext> options, IUserContextService userContext) : base(options)
        {
            _userContext = userContext;
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<LoginHistory> LoginHistories => Set<LoginHistory>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<UserPermission> UserPermissions => Set<UserPermission>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<Poincon> Poincons => Set<Poincon>();
        public DbSet<Utilisation> Utilisations { get; set; }
        public DbSet<Lot> Lots { get; set; }




        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries<AuditableEntity>()
                .Where(e => e.State == EntityState.Added);

            foreach (var entry in entries)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.CreatedBy = _userContext.GetCurrentUserId() ?? "system";
            }

            return await base.SaveChangesAsync(cancellationToken);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();

            modelBuilder.Entity<LoginHistory>()
                .HasOne(l => l.User)
                .WithMany(u => u.LoginHistories)
                .HasForeignKey(l => l.UserId);

            modelBuilder.Entity<UserPermission>()
                .HasKey(up => new { up.UserId, up.PermissionId });

            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.Permission)
                .WithMany(p => p.UserPermissions)
                .HasForeignKey(up => up.PermissionId);
            modelBuilder.Entity<Role>().HasIndex(r => r.Name).IsUnique();

            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany()
                .HasForeignKey(rp => rp.PermissionId);
            // Configure Utilisation-Poincon many-to-many
            modelBuilder.Entity<Utilisation>()
                .HasMany(u => u.Poincons)
                .WithMany(p => p.Utilisations);

            // Configure Utilisation-Lot one-to-many
            modelBuilder.Entity<Utilisation>()
                .HasMany(u => u.Lots)
                .WithOne(l => l.Utilisation)
                .HasForeignKey(l => l.UtilisationId);

            // Configure Utilisation-User many-to-many
            modelBuilder.Entity<Utilisation>()
                .HasMany(u => u.Users)
                .WithMany(u => u.Utilisations);

            modelBuilder.Entity<Utilisation>()
                .HasQueryFilter(u => !u.IsDeleted);


        }
    }

}

