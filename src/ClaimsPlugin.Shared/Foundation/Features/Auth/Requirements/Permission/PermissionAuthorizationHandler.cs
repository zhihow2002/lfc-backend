using System.Security.Claims;
using ClaimsPlugin.Shared.Foundation.Features.Auth.Models;
using ClaimsPlugin.Shared.Foundation.Features.Identity.Configurations;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
using Foundation.Features.Identity.Extensions;
using Foundation.Features.Identity.Interfaces;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Abstractions;
namespace ClaimsPlugin.Shared.Foundation.Features.Auth.Requirements.Permission;
internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    private readonly IApplicationService _applicationService;
    public PermissionAuthorizationHandler(IUserService userService, IHttpContextAccessor httpContextAccessor, IApplicationService applicationService)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
        _applicationService = applicationService;
    }
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        bool isTypeValid = Enum.TryParse(context.User.GetIdentityType(), true, out IdentityType identityType);
        if (!isTypeValid)
        {
            return;
        }
        bool isIdentityValid = Guid.TryParse(context.User.GetIdentityId(), out Guid identityId);
        if (!isIdentityValid)
        {
            return;
        }
        switch (identityType)
        {
            case IdentityType.User:
            {
                bool isRoleValid = Guid.TryParse(_httpContextAccessor.HttpContext?.Request.Headers[IdentityConfiguration.RoleIdFieldName], out Guid roleId);
                if (isRoleValid &&
                    await _userService.HasAccessAsync(
                        identityId,
                        roleId,
                        IdentityConfiguration.IdentityClaimTypes.Permission,
                        requirement.Permission
                    ))
                {
                    context.Succeed(requirement);
                }
                break;
            }
            case IdentityType.Application:
            {
                List<Claim>? scopes = context.User.GetIdentityClaims(OpenIddictConstants.Claims.Private.Scope);
                if(scopes.IsNotNullOrEmpty() && await _applicationService.HasAccessAsync(
                    identityId,
                    scopes,
                    requirement.Permission
                ))
                { 
                    context.Succeed(requirement);
                }
                break;
            }
            default:
                return;
        }
    }
}
