using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PunchSystem.Data;
using PunchSystem.Models;
using PunchSystem.Security;

namespace PunchSystem.Controllers
{
    [Authorize(Policy = PermissionPolicies.ManageUsers)]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            return await _context.Users
                .Include(u => u.Role)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(string id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/permissions")]
        public async Task<IActionResult> GetPermissions(string id)
        {
            var user = await _context.Users
                .Include(u => u.UserPermissions).ThenInclude(up => up.Permission)
                .Include(u => u.Role).ThenInclude(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            var rolePermissions = user.Role.RolePermissions
                .Select(rp => $"{rp.Permission.Module}.{rp.Permission.Action}");

            var userPermissions = user.UserPermissions
                .Select(up => $"{up.Permission.Module}.{up.Permission.Action}");

            var effective = rolePermissions.Union(userPermissions).Distinct();

            return Ok(effective);
        }

        public class AssignPermissionRequest
        {
            public string PermissionId { get; set; } = string.Empty;
        }

        [HttpPost("{id}/permissions")]
        public async Task<IActionResult> AssignPermission(string id, AssignPermissionRequest request)
        {
            var exists = await _context.UserPermissions
                .AnyAsync(up => up.UserId == id && up.PermissionId == request.PermissionId);

            if (exists) return BadRequest("Permission already assigned");

            _context.UserPermissions.Add(new UserPermission
            {
                UserId = id,
                PermissionId = request.PermissionId
            });

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}/permissions/{permissionId}")]
        public async Task<IActionResult> RevokePermission(string id, string permissionId)
        {
            var item = await _context.UserPermissions
                .FirstOrDefaultAsync(up => up.UserId == id && up.PermissionId == permissionId);

            if (item == null) return NotFound();

            _context.UserPermissions.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("connections")]
        public async Task<IActionResult> GetLoginHistory()
        {
            var history = await _context.LoginHistories
                .Include(h => h.User)
                .OrderByDescending(h => h.LoginTime)
                .ToListAsync();

            return Ok(history);
        }
    }
}
