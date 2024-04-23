//using System.Security.Claims;
//using ClaimsPlugin.Shared.Foundation.Features.Auth.Models;
//using Foundation.Features.Identity.Extensions;
//using Microsoft.AspNetCore.Authorization;
//namespace ClaimsPlugin.Shared.Foundation.Features.Auth.Requirements.PasswordReset;
//public class PasswordResetAuthorizationHandler: AuthorizationHandler<PasswordResetRequirement>
//{
//    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PasswordResetRequirement requirement)
//    {
//        if (context.Requirements.Any(x => x is PasswordResetRequirement
//                {
//                    IsMandatory: false
//                }
//            ))
//        {
//            context.Succeed(requirement);
//            return;
//        }
//        if (!requirement.IsMandatory)
//        {
//            context.Succeed(requirement);
//            return;
//        }
//        bool isTypeValid = Enum.TryParse(context.User.GetIdentityType(), true, out IdentityType identityType);
//        if (isTypeValid)
//        {
//            if (identityType == IdentityType.Application)
//            {
//                context.Succeed(requirement);
//                return;
//            }
//            Claim? isPasswordResetRequiredClaim = context.User.Claims.FirstOrDefault(c => c.Type == $"is_password_reset_required");
//            if (isPasswordResetRequiredClaim != null && bool.TryParse(isPasswordResetRequiredClaim.Value, out bool isPasswordResetRequired) && !isPasswordResetRequired)
//            {
//                context.Succeed(requirement);
//                return;
//            }
//            await Task.CompletedTask;
//        }
//        context.Fail();
//    }
//}
