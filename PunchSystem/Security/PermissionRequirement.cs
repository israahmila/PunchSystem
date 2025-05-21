using Microsoft.AspNetCore.Authorization;

namespace PunchSystem.Security
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string PermissionName { get; }

        public PermissionRequirement(string permission)
        {
            PermissionName = permission;
        }
    }
}
