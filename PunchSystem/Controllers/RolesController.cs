using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PunchSystem.Data;
using PunchSystem.Models;
using PunchSystem.Security;

namespace PunchSystem.Controllers
{
    [Authorize(Policy = PermissionPolicies.ManageRoles)]
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RolesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetAll()
        {
            return await _context.Roles.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetById(string id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound();
            return Ok(role);
        }

        [HttpGet("{id}/permissions")]
        public async Task<IActionResult> GetRolePermissions(string id)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (role == null) return NotFound();

            var permissions = role.RolePermissions
                .Select(rp => new
                {
                    rp.PermissionId,
                    Name = $"{rp.Permission.Module}.{rp.Permission.Action}"
                });

            return Ok(permissions);
        }

        public class AssignRolePermissionRequest
        {
            public string PermissionId { get; set; } = string.Empty;
        }

        [HttpPost("{id}/permissions")]
        public async Task<IActionResult> AddPermissionToRole(string id, AssignRolePermissionRequest request)
        {
            var exists = await _context.RolePermissions
                .AnyAsync(rp => rp.RoleId == id && rp.PermissionId == request.PermissionId);

            if (exists) return BadRequest("Permission already assigned");

            _context.RolePermissions.Add(new RolePermission
            {
                RoleId = id,
                PermissionId = request.PermissionId
            });

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}/permissions/{permissionId}")]
        public async Task<IActionResult> RemovePermissionFromRole(string id, string permissionId)
        {
            var item = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == id && rp.PermissionId == permissionId);

            if (item == null) return NotFound();

            _context.RolePermissions.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
