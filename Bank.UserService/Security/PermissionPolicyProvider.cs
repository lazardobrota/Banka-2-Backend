using Bank.Application.Domain;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Bank.UserService.Authorization;

public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
{
    private const string POLICY_PREFIX = "Permission_";
    
    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (!policyName.StartsWith(POLICY_PREFIX))
        {
            return await base.GetPolicyAsync(policyName);
        }

        var permissionsValue = policyName.Substring(POLICY_PREFIX.Length);
        
        var permissionValues = permissionsValue.Split(',')
                                               .Select(p => p.Trim())
                                               .Where(p => !string.IsNullOrEmpty(p));
        
        var permissions = new List<Permission>();
        
        foreach (var permValue in permissionValues)
        {
            if (Enum.TryParse<Permission>(permValue, out var permission))
            {
                permissions.Add(permission);
            }
            else if (long.TryParse(permValue, out var longValue))
            {
                permissions.Add((Permission)longValue);
            }
        }
        
        var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .AddRequirements(new PermissionRequirement(permissions.ToArray()))
                     .Build();
            
        return policy;
    }
}
