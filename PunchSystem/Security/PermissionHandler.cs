﻿using Microsoft.AspNetCore.Authorization;

namespace PunchSystem.Security
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User.HasClaim("Permission", requirement.Permission))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
