//using ClaimsPlugin.Shared.Foundation.Features.Auth.Requirements.ContactConfirmation;
//using ClaimsPlugin.Shared.Foundation.Features.Auth.Requirements.PasswordReset;
//using ClaimsPlugin.Shared.Foundation.Features.Auth.Requirements.Permission;
//using ClaimsPlugin.Shared.Foundation.Features.Auth.Requirements.TenantAccess;
//using ClaimsPlugin.Shared.Foundation.Features.Identity.Configurations;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.Extensions.Options;
//using OpenIddict.Validation.AspNetCore;
//namespace ClaimsPlugin.Shared.Foundation.Features.Auth.Providers;
//internal class AuthorizationPolicyProvider : IAuthorizationPolicyProvider
//{
//    public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
//    {
//        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
//    }
//    public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }
//    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
//    {
//        AuthorizationPolicyBuilder policy = new(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
//        policy.AddRequirements(new PasswordResetRequirement(true));
//        policy.AddRequirements(new ContactConfirmationRequirement(true));
//        return Task.FromResult(policy.RequireAuthenticatedUser().Build());
//    }
//    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
//    {
//        AuthorizationPolicyBuilder policy = new(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
//        if (policyName.Equals(IdentityConfiguration.IdentityPolicies.HasTenantAccess, StringComparison.OrdinalIgnoreCase))
//        {
//            policy.AddRequirements(new TenantAccessRequirement());
//        }
//        if (policyName.StartsWith(IdentityConfiguration.IdentityClaimTypes.Permission, StringComparison.OrdinalIgnoreCase))
//        {
//            policy.AddRequirements(new PermissionRequirement(policyName));
//        }
//        policy.AddRequirements(new PasswordResetRequirement(!policyName.Equals(IdentityConfiguration.IdentityPolicies.NotRequirePasswordReset, StringComparison.OrdinalIgnoreCase)));
//        policy.AddRequirements(new ContactConfirmationRequirement(!policyName.Equals(IdentityConfiguration.IdentityPolicies.NotRequireContactConfirmation, StringComparison.OrdinalIgnoreCase)));
//        if (policy.Requirements.Count > 0)
//        {
//            return Task.FromResult<AuthorizationPolicy?>(policy.RequireAuthenticatedUser().Build());
//        }
//        return FallbackPolicyProvider.GetPolicyAsync(policyName);
//    }
//    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
//    {
//        return Task.FromResult<AuthorizationPolicy?>(null);
//    }
//}
