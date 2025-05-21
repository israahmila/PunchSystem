using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PunchSystem.Data;
using System.Security.Claims;

namespace PunchSystem.Security
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _http;

        public PermissionHandler(AppDbContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // 1. Récupérer UserId du token
            var userId = _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                context.Fail();
                return;
            }

            // 2. Récupérer utilisateur + permissions
            var user = await _context.Users
                .Include(u => u.UserPermissions).ThenInclude(up => up.Permission)
                .Include(u => u.Role).ThenInclude(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || !user.IsActive)
            {
                context.Fail();
                return;
            }

            // 3. Calculer les permissions effectives
            var rolePerms = user.Role.RolePermissions
                .Select(rp => $"{rp.Permission.Module}.{rp.Permission.Action}");

            var userPerms = user.UserPermissions
                .Select(up => $"{up.Permission.Module}.{up.Permission.Action}");

            var allPerms = rolePerms.Union(userPerms).ToHashSet();

            // 4. Vérifier si permission demandée est présente
            if (allPerms.Contains(requirement.PermissionName))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }

}
