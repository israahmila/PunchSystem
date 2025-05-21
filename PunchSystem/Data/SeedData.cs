using PunchSystem.Helpers;
using PunchSystem.Models;

namespace PunchSystem.Data
{
    public static class DbInitializer
    {
        public static void SeedRoles(AppDbContext context)
        {
            if (context.Roles.Any()) return;

            // 📌 1. Créer les rôles
            var roles = new[]
            {
                new Role { Id = IdGenerator.New("ROL"), Name = "Administrateur" },
                new Role { Id = IdGenerator.New("ROL"), Name = "Superviseur" },
                new Role { Id = IdGenerator.New("ROL"), Name = "Ouvrier" },
                new Role { Id = IdGenerator.New("ROL"), Name = "AgentSaisie" }
            };

            context.Roles.AddRange(roles);

            // 📌 2. Créer les permissions
            var permissions = new List<Permission>
            {
                new Permission { Id = IdGenerator.New("PER"), Module = "User", Action = "Manage" },
                new Permission { Id = IdGenerator.New("PER"), Module = "Role", Action = "Manage" },
                new Permission { Id = IdGenerator.New("PER"), Module = "Produit", Action = "Manage" },
                new Permission { Id = IdGenerator.New("PER"), Module = "Poincon", Action = "Manage" },
                new Permission { Id = IdGenerator.New("PER"), Module = "Fournisseur", Action = "Manage" },
                new Permission { Id = IdGenerator.New("PER"), Module = "Marque", Action = "Manage" },
                new Permission { Id = IdGenerator.New("PER"), Module = "Utilisation", Action = "Create" },
                new Permission { Id = IdGenerator.New("PER"), Module = "Audit", Action = "View" }
            };

            context.Permissions.AddRange(permissions);
            context.SaveChanges();

            // 📌 3. Associer rôles à permissions
            List<RolePermission> rolePermissions = new();

            void Assign(string roleName, string permissionName)
            {
                var role = roles.First(r => r.Name == roleName);
                var perm = permissions.First(p => $"{p.Module}.{p.Action}" == permissionName);
                rolePermissions.Add(new RolePermission
                {
                    RoleId = role.Id,
                    PermissionId = perm.Id
                });
            }

            // 🎯 Associations
            // Admin = tout
            foreach (var perm in permissions)
                Assign("Administrateur", $"{perm.Module}.{perm.Action}");

            // Superviseur
            Assign("Superviseur", "Produit.Manage");
            Assign("Superviseur", "Poincon.Manage");
            Assign("Superviseur", "Fournisseur.Manage");
            Assign("Superviseur", "Marque.Manage");
            Assign("Superviseur", "Utilisation.Create");

            // Ouvrier
            Assign("Ouvrier", "Poincon.Manage");
            Assign("Ouvrier", "Fournisseur.Manage");
            Assign("Ouvrier", "Marque.Manage");
            Assign("Ouvrier", "Utilisation.Create");

            // Agent de saisie
            Assign("AgentSaisie", "Utilisation.Create");

            context.RolePermissions.AddRange(rolePermissions);
            context.SaveChanges();
        }
    }
}
