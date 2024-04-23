//using ClaimsPlugin.Shared.Foundation.Features.Auth.Models;
//using ClaimsPlugin.Shared.Foundation.Features.Identity.Configurations;
//using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
//using Foundation.Features.Identity.Extensions;
//using Foundation.Features.Identity.Interfaces;
//using Microsoft.AspNetCore.Authorization;

//namespace ClaimsPlugin.Shared.Foundation.Features.Auth.Requirements.TenantAccess;

//internal class TenantAccessAuthorizationHandler : AuthorizationHandler<TenantAccessRequirement>
//{
//    private readonly IHttpContextAccessor _httpContextAccessor;
//    private readonly IUserService _userService;

//    public TenantAccessAuthorizationHandler(IUserService userService, IHttpContextAccessor httpContextAccessor)
//    {
//        _userService = userService;
//        _httpContextAccessor = httpContextAccessor;
//    }

//    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TenantAccessRequirement requirement)
//    {
//        bool isTypeValid = Enum.TryParse(context.User.GetIdentityType(), true, out IdentityType identityType);

//        if (!isTypeValid)
//        {
//            return;
//        }
        
//        bool isIdentityValid = Guid.TryParse(context.User.GetIdentityId(), out Guid identityId);

//        if (!isIdentityValid)
//        {
//            return;
//        }
        
//        switch (identityType)
//        {
//            case IdentityType.User:
//            {
//                string? tenantHeader = _httpContextAccessor.HttpContext?.Request.Headers[IdentityConfiguration.IdentityClaimTypes.Tenant];

//                if (tenantHeader.IsNotNullOrWhiteSpace())
//                {
//                    bool isRoleValid = Guid.TryParse(_httpContextAccessor.HttpContext?.Request.Headers[IdentityConfiguration.RoleIdFieldName], out Guid roleId);

//                    if (isIdentityValid && isRoleValid && await _userService.HasAccessAsync(identityId, roleId, IdentityConfiguration.IdentityClaimTypes.Tenant, tenantHeader))
//                    {
//                        context.Succeed(requirement);
//                    }
//                }

//                break;
//            }
//            case IdentityType.Application:
//            {
//                context.Succeed(requirement);
//                break;
//            }
//            default:
//                return;
//        }
//    }
//}
