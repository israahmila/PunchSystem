using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PunchSystem.Data;
using PunchSystem.DTOs;
using PunchSystem.Models;

namespace PunchSystem.Controllers
{
    public class PermissionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PermissionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/permissions
        [HttpGet]
        public async Task<IActionResult> GetAllPermissions()
        {
            var permissions = await _context.Permissions
                .Select(p => p.Name)
                .ToListAsync();

            return Ok(permissions);
        }

        // GET: api/permissions/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserPermissions(int userId)
        {
            var userPermissions = await _context.UserPermissions
                .Where(up => up.UserId == userId)
                .Include(up => up.Permission)
                .Select(up => up.Permission.Name)
                .ToListAsync();

            return Ok(userPermissions);
        }

        // POST: api/permissions/assign
        [HttpPost("assign")]
        public async Task<IActionResult> AssignPermissions([FromBody] AssignPermissionRequest request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
                return NotFound("User not found");

            var allPermissions = await _context.Permissions
                .Where(p => request.Permissions.Contains(p.Name))
                .ToListAsync();

            foreach (var permission in allPermissions)
            {
                var exists = await _context.UserPermissions.AnyAsync(up =>
                    up.UserId == user.Id && up.PermissionId == permission.Id);

                if (!exists)
                {
                    _context.UserPermissions.Add(new UserPermission
                    {
                        UserId = user.Id,
                        PermissionId = permission.Id
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Ok("Permissions assigned.");
        }

        // POST: api/permissions/remove
        [HttpPost("remove")]


        public async Task<IActionResult> RemovePermissions([FromBody] AssignPermissionRequest request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
                return NotFound("User not found");

            var toRemove = await _context.UserPermissions
                .Where(up => up.UserId == request.UserId &&
                             request.Permissions.Contains(up.Permission.Name))
                .Include(up => up.Permission)
                .ToListAsync();

            _context.UserPermissions.RemoveRange(toRemove);
            await _context.SaveChangesAsync();

            return Ok("Permissions removed.");
        }
    }
}
