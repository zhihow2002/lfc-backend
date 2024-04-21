using ClaimsPlugin.Shared.Foundation.Features.Identity.Configurations;
using Microsoft.AspNetCore.Authorization;

namespace ClaimsPlugin.Shared.Foundation.Features.Auth.Requirements.TenantAccess;

[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Method,
    AllowMultiple = false,
    Inherited = true
)]
public class HasTenantAccessControlAttribute : AuthorizeAttribute
{
    public HasTenantAccessControlAttribute()
        : base(IdentityConfiguration.IdentityPolicies.HasTenantAccess) { }
}
