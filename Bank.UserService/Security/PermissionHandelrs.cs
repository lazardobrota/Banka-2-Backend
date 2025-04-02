using Bank.Application.Domain;

using Microsoft.AspNetCore.Authorization;

namespace Bank.UserService.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public Permission[] Permissions { get; }

    public PermissionRequirement(Permission[] permissions)
    {
        Permissions = permissions;
    }
}

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var permissionClaim = context.User.FindFirst("permission");

        if (permissionClaim is not null && long.TryParse(permissionClaim.Value, out var permissionValue))
        {
            var userPermissions = new AuthPermission(permissionValue);

            if (requirement.Permissions.Any(userPermissions.HasPermission))
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}

// public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
// {
//     protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
//     {
//         var permissionsClaim = context.User.FindFirst("permission");
//         if (permissionsClaim == null)
//         {
//             return Task.CompletedTask;
//         }
//         
//         if (long.TryParse(permissionsClaim.Value, out var userPermissionsValue))
//         {
//             var userPermissions = new AuthPermission(userPermissionsValue);
//             
//             // Check if the user has any of the required permissions
//             foreach (var permission in requirement.Permissions)
//             {
//                 if (userPermissions.HasPermission(permission))
//                 {
//                     context.Succeed(requirement);
//                     return Task.CompletedTask;
//                 }
//             }
//         }
//         
//         return Task.CompletedTask;
//     }
// }
