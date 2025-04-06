using Bank.Application.Domain;

using Microsoft.AspNetCore.Authorization;

namespace Bank.UserService.Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class RequirePermissionAttribute : AuthorizeAttribute
{
    public RequirePermissionAttribute(Permission requiredPermission)
    {
        Policy = $"Permission_{(long)requiredPermission}";
    }
}
