using System.Security.Claims;
using ClaimsPlugin.Shared.Foundation.Common.Utilities;
using ClaimsPlugin.Shared.Foundation.Features.Auth.Models;
using Foundation.Features.Identity.Extensions;
using Foundation.Features.Identity.ValueObjects;
using Microsoft.AspNetCore.Authorization;
namespace ClaimsPlugin.Shared.Foundation.Features.Auth.Requirements.ContactConfirmation;
public class ContactConfirmationAuthorizationHandler: AuthorizationHandler<ContactConfirmationRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ContactConfirmationRequirement requirement)
    {
        if (context.Requirements.Any(x => x is ContactConfirmationRequirement
                {
                    IsMandatory: false
                }
            ))
        {
            context.Succeed(requirement);
            return;
        }
        if (!requirement.IsMandatory)
        {
            context.Succeed(requirement);
            return;
        }
        bool isTypeValid = Enum.TryParse(context.User.GetIdentityType(), true, out IdentityType identityType);
        if (isTypeValid)
        {
            if (identityType == IdentityType.Application)
            {
                context.Succeed(requirement);
                return;
            }
            Claim? isEmailConfirmedClaim = context.User.Claims.FirstOrDefault(c => c.Type == $"is_{AuthenticationMethod.Email.Value.RemoveWhitespaces().ToSnakeCase()}_confirmed");
            Claim? isPhoneNumberConfirmedClaim = context.User.Claims.FirstOrDefault(c => c.Type == $"is_{AuthenticationMethod.PhoneNumber.Value.RemoveWhitespaces().ToSnakeCase()}_confirmed");
            if (isEmailConfirmedClaim != null && bool.TryParse(isEmailConfirmedClaim.Value, out bool isEmailConfirmed) && isEmailConfirmed)
            {
                context.Succeed(requirement);
                return;
            }
            if (isPhoneNumberConfirmedClaim != null && bool.TryParse(isPhoneNumberConfirmedClaim.Value, out bool isPhoneNumberConfirmed) && isPhoneNumberConfirmed)
            {
                context.Succeed(requirement);
                return;
            }
            await Task.CompletedTask;
        }
        context.Fail();
    }
}
