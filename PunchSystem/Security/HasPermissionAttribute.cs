using Microsoft.AspNetCore.Authorization;

namespace PunchSystem.Security
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(string permission) : base(policy: permission) { }
    }
}
