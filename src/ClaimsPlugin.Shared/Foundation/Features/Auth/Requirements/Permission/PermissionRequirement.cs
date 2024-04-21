using Microsoft.AspNetCore.Authorization;

namespace ClaimsPlugin.Shared.Foundation.Features.Auth.Requirements.Permission;

internal class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }

    public string Permission { get; }
}
