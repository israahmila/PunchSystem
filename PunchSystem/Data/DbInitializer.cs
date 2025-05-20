using PunchSystem.Models;

namespace PunchSystem.Data
{
    public static class DbInitializer
    {
        public static void SeedRoles(AppDbContext context)
        {
            context.Database.EnsureCreated();
            if (!context.Roles.Any())
            {
                var roles = new List<Role>
                {
                    new Role { Id = "ROL-ADMIN", Name = "Admin" },
                    new Role { Id = "ROL-OP", Name = "Operator" }
                };

                context.Roles.AddRange(roles);
                context.SaveChanges();
            }
        }
    }
}
