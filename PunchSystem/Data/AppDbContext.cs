using Microsoft.EntityFrameworkCore;
using PunchSystem.Contracts;
using PunchSystem.Models;

namespace PunchSystem.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IUserContextService _userContext;

        public AppDbContext(DbContextOptions<AppDbContext> options, IUserContextService userContext)
            : base(options)
        {
            _userContext = userContext;
        }

        // 📦 DbSets principaux
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }

        public DbSet<Produit> Produits { get; set; }
        public DbSet<Poincon> Poincons { get; set; }
        public DbSet<Fournisseur> Fournisseurs { get; set; }
        public DbSet<Marque> Marques { get; set; }

        public DbSet<Utilisation> Utilisations { get; set; }
        public DbSet<Lot> Lots { get; set; }

        public DbSet<Entretien> Entretiens { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }
        public DbSet<LoginHistory> LoginHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Configuration clé composée : RolePermission
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);

            // ✅ Configuration clé composée : UserPermission
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

            // ✅ Many-to-Many: Utilisation <-> User
            modelBuilder.Entity<Utilisation>()
    .HasMany(u => u.Users)
    .WithMany(u => u.Utilisations)
    .UsingEntity(j => j.ToTable("UtilisationUsers"));


            // ✅ Many-to-Many: Utilisation <-> Poincon
            modelBuilder.Entity<Utilisation>()
    .HasMany(u => u.Poincons)
    .WithMany(p => p.Utilisations)
    .UsingEntity(j => j.ToTable("UtilisationPoincons"));
        }

        // Optionnel : override SaveChanges pour l'audit (CreatedAt, UpdatedAt)
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<AuditableEntity>();

            foreach (var entry in entries)
            {
                var userId = _userContext.GetCurrentUserId() ?? "SYSTEM";

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = userId; // ✅ fix ici
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = userId;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }


    }
}
