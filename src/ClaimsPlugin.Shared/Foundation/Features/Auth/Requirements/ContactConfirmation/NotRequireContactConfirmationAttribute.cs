using ClaimsPlugin.Shared.Foundation.Features.Identity.Configurations;
using Microsoft.AspNetCore.Authorization;
namespace ClaimsPlugin.Shared.Foundation.Features.Auth.Requirements.ContactConfirmation;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class NotRequireContactConfirmationAttribute : AuthorizeAttribute
{
    public NotRequireContactConfirmationAttribute() : base(IdentityConfiguration.IdentityPolicies.NotRequireContactConfirmation)
    {
    }
}
