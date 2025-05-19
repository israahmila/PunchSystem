using Microsoft.EntityFrameworkCore;
using PunchSystem.Data;
using PunchSystem.Models;

namespace PunchSystem.Services
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;

        public DataSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Seed Permissions
            if (!_context.Permissions.Any())
            {
                var allPermissions = new[]
                {
            "EditUsers",
            "DeleteUsers",
            "CreateProduct",
            "DeleteProduct",
            "ViewReports",
            "AssignPermissions"
        }.Select(p => new Permission { Name = p }).ToList();

                _context.Permissions.AddRange(allPermissions);
                await _context.SaveChangesAsync();
            }

            // Seed Roles
            if (!_context.Roles.Any())
            {
                var adminRole = new Role { Name = "Admin" };
                var supervisor = new Role { Name = "Superviseur" };
                var worker = new Role { Name = "Ouvrier" };
                var inputAgent = new Role { Name = "Agent" };

                _context.Roles.AddRange(adminRole, supervisor, worker, inputAgent);
                await _context.SaveChangesAsync();

                // Attach permissions to each role
                var allPerms = await _context.Permissions.ToListAsync();

                var adminPerms = allPerms; // full access
                var supervisorPerms = allPerms.Where(p => p.Name != "AssignPermissions").ToList();
                var workerPerms = allPerms.Where(p => p.Name == "ViewReports").ToList();
                var agentPerms = new List<Permission>(); // No default permissions

                _context.RolePermissions.AddRange(adminPerms.Select(p => new RolePermission { RoleId = adminRole.Id, PermissionId = p.Id }));
                _context.RolePermissions.AddRange(supervisorPerms.Select(p => new RolePermission { RoleId = supervisor.Id, PermissionId = p.Id }));
                _context.RolePermissions.AddRange(workerPerms.Select(p => new RolePermission { RoleId = worker.Id, PermissionId = p.Id }));
            }

            await _context.SaveChangesAsync();

            // Seed Admin User
            if (!_context.Users.Any(u => u.Username == "admin"))
            {
                var adminRole = await _context.Roles.FirstAsync(r => r.Name == "Admin");

                var admin = new User
                {
                    Id = 0, // Auto-incremented by the database
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    RoleId = adminRole.Id,
                    IsActive = true
                };

                _context.Users.Add(admin);
                await _context.SaveChangesAsync();
            }
        }

    }
}
