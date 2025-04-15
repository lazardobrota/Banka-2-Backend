using Bank.Permissions.Core;
using Bank.Permissions.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

using IAuthorizationService = Bank.Permissions.Services.IAuthorizationService;

namespace Bank.Permissions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorizationServices(this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddAuthorizationBuilder()
                .SetDefaultPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                                  .Build());

        services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
        services.AddSingleton<IAuthorizationService, AuthorizationService>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

        return services;
    }
}
