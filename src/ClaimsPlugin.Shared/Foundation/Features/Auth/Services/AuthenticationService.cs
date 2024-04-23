//using System.Collections.Immutable;
//using System.Security.Claims;
//using ClaimsPlugin.Shared.Foundation.Common.Utilities;
//using ClaimsPlugin.Shared.Foundation.Features.Auth.Models;
//using ClaimsPlugin.Shared.Foundation.Features.Identity.Configurations;
//using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
//using Foundation.Features.Identity.Entities;
//using Foundation.Features.Identity.Interfaces;
//using Foundation.Features.Identity.ValueObjects;
//using Microsoft.AspNetCore;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Localization;
//using Microsoft.IdentityModel.Tokens;
//using OpenIddict.Abstractions;
//using OpenIddict.Core;
//using OpenIddict.EntityFrameworkCore.Models;
//using OpenIddict.Server.AspNetCore;
//using UserLoginInfo = Foundation.Features.Identity.Entities.UserLoginInfo;
//namespace ClaimsPlugin.Shared.Foundation.Features.Auth.Services;
//public class AuthenticationService : Interfaces.IAuthenticationService
//{
//    private readonly IUserService _userService;
//    private readonly IApplicationService _applicationService;
//    private readonly IStringLocalizer<AuthenticationService> _localizer;
//    private readonly OpenIddictScopeManager<OpenIddictEntityFrameworkCoreScope<Guid>> _scopeManager;
//    private readonly UserManager<User> _userManager;
//    private readonly SignInManager<User> _signInManager;
//    public AuthenticationService(
//        IUserService userService,
//        IApplicationService applicationService,
//        IStringLocalizer<AuthenticationService> localizer,
//        OpenIddictScopeManager<OpenIddictEntityFrameworkCoreScope<Guid>> scopeManager,
//        UserManager<User> userManager,
//        SignInManager<User> signInManager
//    )
//    {
//        _userService = userService;
//        _applicationService = applicationService;
//        _localizer = localizer;
//        _scopeManager = scopeManager;
//        _userManager = userManager;
//        _signInManager = signInManager;
//    }
//    public async Task<IActionResult> AuthenticateAsync(
//        HttpContext httpContext,
//        CancellationToken cancellationToken = default
//    )
//    {
//        Dictionary<string, string>? messages = null;
//        OpenIddictRequest? request = httpContext.GetOpenIddictServerRequest();
//        if (request.IsNull())
//        {
//            return LoginFailed(
//                messages
//                    ?? new Dictionary<string, string>
//                    {
//                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants
//                            .Errors
//                            .InvalidRequest,
//                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
//                            _localizer["identity.openId.requestCannotBeRetrieved"]
//                    }
//            );
//        }
//        if (request.IsPasswordGrantType())
//        {
//            (ClaimsPrincipal? principal, Dictionary<string, string>? messages) result =
//                await UsePasswordFlow(request, cancellationToken);
//            if (result.principal.IsNotNull())
//            {
//                return LoginSuccessful(result.principal);
//            }
//            messages = result.messages;
//        }
//        if (request.IsRefreshTokenGrantType())
//        {
//            (ClaimsPrincipal? principal, Dictionary<string, string>? messages) result =
//                await UseRefreshTokenFlow(httpContext, request, cancellationToken);
//            if (result.principal.IsNotNull())
//            {
//                return LoginSuccessful(result.principal);
//            }
//            messages = result.messages;
//        }
//        if (request.IsClientCredentialsGrantType())
//        {
//            (ClaimsPrincipal? principal, Dictionary<string, string>? messages) result =
//                await UseClientCredentialFlow(request, cancellationToken);
//            if (result.principal.IsNotNull())
//            {
//                return LoginSuccessful(result.principal);
//            }
//            messages = result.messages;
//        }
//        return LoginFailed(
//            messages
//                ?? new Dictionary<string, string>
//                {
//                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants
//                        .Errors
//                        .InvalidGrant,
//                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = _localizer[
//                        "identity.openId.specifiedGrantTypeNotSupported"
//                    ]
//                }
//        );
//    }
//    private async Task<(
//        ClaimsPrincipal? principal,
//        Dictionary<string, string>? messages
//    )> UsePasswordFlow(OpenIddictRequest request, CancellationToken cancellationToken = default)
//    {
//        if (request.Username.IsNull() || request.Password.IsNull())
//        {
//            return (
//                null,
//                new Dictionary<string, string>
//                {
//                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants
//                        .Errors
//                        .InvalidRequest,
//                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = _localizer[
//                        "identity.openId.authenticationFailed"
//                    ]
//                }
//            );
//        }
//        UserLoginInfo? userLoginInfo = await _userService.GetUserLoginInfoFromLoginInfoAsync(
//            request.Username.Trim().ToUpperInvariant()
//        );
//        if (userLoginInfo.IsNull() || userLoginInfo.User.IsNull())
//        {
//            return (
//                null,
//                new Dictionary<string, string>
//                {
//                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants
//                        .Errors
//                        .InvalidRequest,
//                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = _localizer[
//                        "identity.openId.authenticationFailed"
//                    ]
//                }
//            );
//        }
//        (bool isSuccess, string? errorMessage) signInRequirement =
//            await ValidateSignInRequirementsAsync(userLoginInfo);
//        if (!signInRequirement.isSuccess)
//        {
//            return (
//                null,
//                new Dictionary<string, string>
//                {
//                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants
//                        .Errors
//                        .InvalidRequest,
//                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
//                        signInRequirement.errorMessage!
//                }
//            );
//        }
//        (bool isSuccess, string? errorMessage) userStatus = await ValidateUserStatusAsync(
//            userLoginInfo.User
//        );
//        if (!userStatus.isSuccess)
//        {
//            return (
//                null,
//                new Dictionary<string, string>
//                {
//                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants
//                        .Errors
//                        .InvalidRequest,
//                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
//                        userStatus.errorMessage!
//                }
//            );
//        }
//        (bool isSuccess, string? errorMessage) userPassword = await ValidateUserPasswordAsync(
//            userLoginInfo.User,
//            request.Password
//        );
//        if (!userPassword.isSuccess)
//        {
//            return (
//                null,
//                new Dictionary<string, string>
//                {
//                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants
//                        .Errors
//                        .InvalidRequest,
//                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
//                        userPassword.errorMessage!
//                }
//            );
//        }
//        if (userLoginInfo.User.TwoFactorEnabled)
//        {
//            if (request.Code.IsNullOrWhiteSpace())
//            {
//                await _userService.RequestOtpByLoginIdAsync(
//                    request.Username.Trim().ToUpperInvariant(),
//                    string.Empty,
//                    cancellationToken
//                );
//                return (
//                    null,
//                    new Dictionary<string, string>
//                    {
//                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = "mfa_required",
//                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
//                            _localizer["identity.multiFactorAuthenticationRequired"]
//                    }
//                );
//            }
//            if (
//                !await _userService.VerifyOtpByLoginIdAsync(
//                    request.Username.Trim().ToUpperInvariant(),
//                    request.Code,
//                    cancellationToken
//                )
//            )
//            {
//                return (
//                    null,
//                    new Dictionary<string, string>
//                    {
//                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = "invalid_code",
//                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
//                            _localizer["identity.invalidOtp"]
//                    }
//                );
//            }
//        }
//        ClaimsIdentity identity =
//            new(
//                TokenValidationParameters.DefaultAuthenticationType,
//                OpenIddictConstants.Claims.Name,
//                OpenIddictConstants.Claims.Role
//            );
//        identity
//            .AddClaim(OpenIddictConstants.Claims.Subject, userLoginInfo.User.Id.ToString())
//            .AddClaim(OpenIddictConstants.Claims.Name, userLoginInfo.User.PersonName)
//            .AddClaims(
//                OpenIddictConstants.Claims.Role,
//                (await _userService.GetAssignedRolesAsync(userLoginInfo.User.Id, cancellationToken))
//                    .Select(x => x.RoleId.ToString())
//                    .ToImmutableArray()
//            );
//        List<UserLoginInfo>? allLoginInfos = await _userService.GetUserLoginInfoFromLoginInfosAsync(
//            userLoginInfo.UserId,
//            cancellationToken
//        );
//        identity.AddClaim(
//            IdentityConfiguration.IdentityClaims.IdentityType,
//            IdentityType.User.ToString(),
//            OpenIddictConstants.Destinations.AccessToken
//        );
//        foreach (AuthenticationMethod supportedItem in AuthenticationMethod.SupportedItems)
//        {
//            Claim claim =
//                new(
//                    $"is_{supportedItem.Value.RemoveWhitespaces().ToSnakeCase()}_confirmed",
//                    (
//                        allLoginInfos?.FirstOrDefault(
//                            x => x.IsConfirmed && x.AuthenticationMethod == supportedItem
//                        ) != null
//                    ).ToString(),
//                    ClaimValueTypes.Boolean,
//                    OpenIddictConstants.Destinations.AccessToken,
//                    OpenIddictConstants.Destinations.AccessToken,
//                    identity
//                );
//            claim.Properties.Add(".destinations", OpenIddictConstants.Destinations.AccessToken);
//            identity.AddClaim(claim);
//        }
//        Claim passwordResetClaim =
//            new(
//                $"is_password_reset_required",
//                userLoginInfo.User.IsPasswordResetRequired.ToString(),
//                ClaimValueTypes.Boolean,
//                OpenIddictConstants.Destinations.AccessToken,
//                OpenIddictConstants.Destinations.AccessToken,
//                identity
//            );
//        passwordResetClaim.Properties.Add(
//            ".destinations",
//            OpenIddictConstants.Destinations.AccessToken
//        );
//        identity.AddClaim(passwordResetClaim);
//        ClaimsPrincipal principal = new(identity);
//        principal.SetScopes(
//            new[]
//            {
//                OpenIddictConstants.Scopes.OpenId,
//                OpenIddictConstants.Scopes.OfflineAccess,
//                OpenIddictConstants.Scopes.Roles
//            }.Intersect(request.GetScopes())
//        );
//        principal.SetResources(
//            (
//                await _scopeManager
//                    .ListResourcesAsync(principal.GetScopes(), cancellationToken)
//                    .ToListAsync(cancellationToken: cancellationToken)
//            ).Concat(
//                new[]
//                {
//                    IdentityConfiguration.DefaultIntrospector.ClientId
//                }
//            )
//        );
//        principal.SetDestinations(claim => GetDestinations(claim));
//        return (principal, null);
//    }
//    private async Task<(
//        ClaimsPrincipal? principal,
//        Dictionary<string, string>? messages
//    )> UseRefreshTokenFlow(
//        HttpContext httpContext,
//        OpenIddictRequest request,
//        CancellationToken cancellationToken = default
//    )
//    {
//        ClaimsPrincipal? principal = (
//            await httpContext.AuthenticateAsync(
//                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
//            )
//        ).Principal;
//        if (
//            principal.IsNull()
//            || !principal.HasClaim(IdentityConfiguration.IdentityClaims.IdentityType)
//        )
//        {
//            return (
//                null,
//                new Dictionary<string, string>
//                {
//                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants
//                        .Errors
//                        .InvalidToken,
//                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = _localizer[
//                        "identity.invalidToken"
//                    ]
//                }
//            );
//        }
//        Claim? claim = principal.Claims.FirstOrDefault(
//            x => x.Type == IdentityConfiguration.IdentityClaims.IdentityType
//        );
//        if (claim.IsNull())
//        {
//            return (
//                null,
//                new Dictionary<string, string>
//                {
//                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants
//                        .Errors
//                        .InvalidToken,
//                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = _localizer[
//                        "identity.invalidToken"
//                    ]
//                }
//            );
//        }
//        if (claim.Value == IdentityType.User.ToString())
//        {
//            User? user = await _userService.GetFromClaimsPrincipalAsync(
//                principal,
//                cancellationToken
//            );
//            if (user.IsNull())
//            {
//                return (
//                    null,
//                    new Dictionary<string, string>
//                    {
//                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants
//                            .Errors
//                            .InvalidToken,
//                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
//                            _localizer["identity.invalidToken"]
//                    }
//                );
//            }
//            (bool isSuccess, string? errorMessage) userStatus = await ValidateUserStatusAsync(user);
//            if (!userStatus.isSuccess)
//            {
//                return (
//                    null,
//                    new Dictionary<string, string>
//                    {
//                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants
//                            .Errors
//                            .InvalidRequest,
//                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
//                            userStatus.errorMessage!
//                    }
//                );
//            }
//            if (principal.Identity is not ClaimsIdentity originalIdentity)
//            {
//                return (
//                    null,
//                    new Dictionary<string, string>
//                    {
//                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants
//                            .Errors
//                            .InvalidToken,
//                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
//                            _localizer["identity.invalidToken"]
//                    }
//                );
//            }
//            ClaimsIdentity newIdentity = new(originalIdentity.AuthenticationType);
//            foreach (Claim originalClaim in originalIdentity.Claims)
//            {
//                AuthenticationMethod? methodClaim =
//                    AuthenticationMethod.SupportedItems.SingleOrDefault(
//                        x =>
//                            $"is_{x.Value.RemoveWhitespaces().ToSnakeCase()}_confirmed"
//                            == originalClaim.Type
//                    );
//                if (methodClaim.IsNotNull())
//                {
//                    Claim updatedClaim =
//                        new(
//                            originalClaim.Type,
//                            (
//                                user.UserLoginInfos.FirstOrDefault(
//                                    x => x.IsConfirmed && x.AuthenticationMethod == methodClaim
//                                ) != null
//                            ).ToString(),
//                            originalClaim.ValueType,
//                            originalClaim.Issuer,
//                            originalClaim.OriginalIssuer,
//                            originalClaim.Subject
//                        );
//                    updatedClaim.Properties.Add(
//                        ".destinations",
//                        OpenIddictConstants.Destinations.AccessToken
//                    );
//                    newIdentity.AddClaim(updatedClaim);
//                }
//                else
//                {
//                    newIdentity.AddClaim(originalClaim);
//                }
//            }
//            principal = new ClaimsPrincipal(newIdentity);
//        }
//        if (claim.Value == IdentityType.Application.ToString())
//        {
//            ApplicationDataTransferObject? application =
//                await _applicationService.GetByClientIdAsync(request.ClientId!, cancellationToken);
//            if (application.IsNull())
//            {
//                return (
//                    null,
//                    new Dictionary<string, string>
//                    {
//                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants
//                            .Errors
//                            .InvalidToken,
//                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
//                            _localizer["identity.invalidToken"]
//                    }
//                );
//            }
//        }
//        principal.SetDestinations(principalClaim => GetDestinations(principalClaim));
//        return (principal, null);
//    }
//    private async Task<(
//        ClaimsPrincipal? principal,
//        Dictionary<string, string>? messages
//    )> UseClientCredentialFlow(
//        OpenIddictRequest request,
//        CancellationToken cancellationToken = default
//    )
//    {
//        ApplicationDataTransferObject? application = await _applicationService.GetByClientIdAsync(
//            request.ClientId!,
//            cancellationToken
//        );
//        if (application.IsNull())
//        {
//            return (
//                null,
//                new Dictionary<string, string>
//                {
//                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants
//                        .Errors
//                        .InvalidClient,
//                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = _localizer[
//                        "identity.applicationNotFound"
//                    ]
//                }
//            );
//        }
//        ClaimsIdentity identity =
//            new(
//                TokenValidationParameters.DefaultAuthenticationType,
//                OpenIddictConstants.Claims.Name,
//                OpenIddictConstants.Claims.Role
//            );
//        identity.AddClaim(
//            OpenIddictConstants.Claims.Subject,
//            application.Id.ToString() ?? throw new InvalidOperationException()
//        );
//        identity.AddClaim(
//            IdentityConfiguration.IdentityClaims.IdentityType,
//            IdentityType.Application.ToString(),
//            OpenIddictConstants.Destinations.AccessToken
//        );
//        ClaimsPrincipal principal = new(identity);
//        principal.SetScopes(request.GetScopes());
//        principal.SetResources(
//            (
//                await _scopeManager
//                    .ListResourcesAsync(principal.GetScopes(), cancellationToken)
//                    .ToListAsync(cancellationToken: cancellationToken)
//            ).Concat(
//                new[]
//                {
//                    IdentityConfiguration.DefaultIntrospector.ClientId
//                }
//            )
//        );
//        principal.SetDestinations(claim => GetDestinations(claim));
//        return (principal, null);
//    }
//    private static IEnumerable<string> GetDestinations(
//        Claim claim,
//        ClaimsPrincipal? principal = null
//    )
//    {
//        switch (claim.Type)
//        {
//            case OpenIddictConstants.Claims.Role:
//                yield return OpenIddictConstants.Destinations.AccessToken;
//                if (
//                    principal != null
//                    && principal.HasScope(OpenIddictConstants.Permissions.Scopes.Roles)
//                )
//                {
//                    yield return OpenIddictConstants.Destinations.IdentityToken;
//                }
//                yield break;
//            case OpenIddictConstants.Claims.Name:
//                yield return OpenIddictConstants.Destinations.AccessToken;
//                yield break;
//            case IdentityConfiguration.IdentityClaims.IdentityType:
//                yield return OpenIddictConstants.Destinations.AccessToken;
//                yield break;
//            case "AspNet.Identity.SecurityStamp":
//                yield break;
//            default:
//                if (claim.Properties.ContainsKey(".destinations"))
//                {
//                    if (
//                        claim
//                            .Properties[".destinations"]
//                            .Contains(OpenIddictConstants.Destinations.AccessToken)
//                    )
//                    {
//                        yield return OpenIddictConstants.Destinations.AccessToken;
//                    }
//                    if (
//                        claim
//                            .Properties[".destinations"]
//                            .Contains(OpenIddictConstants.Destinations.IdentityToken)
//                    )
//                    {
//                        yield return OpenIddictConstants.Destinations.IdentityToken;
//                    }
//                    yield break;
//                }
//                if (claim.Properties.ContainsKey("IncludeInAccessToken"))
//                {
//                    if (
//                        bool.TryParse(
//                            claim.Properties["IncludeInAccessToken"],
//                            out bool includeInAccessToken
//                        ) && includeInAccessToken
//                    )
//                    {
//                        yield return OpenIddictConstants.Destinations.AccessToken;
//                    }
//                }
//                if (claim.Properties.ContainsKey("IncludeInIdentityToken"))
//                {
//                    if (
//                        bool.TryParse(
//                            claim.Properties["IncludeInIdentityToken"],
//                            out bool includeInIdentityToken
//                        ) && includeInIdentityToken
//                    )
//                    {
//                        yield return OpenIddictConstants.Destinations.IdentityToken;
//                    }
//                }
//                yield break;
//        }
//    }
//    private static IActionResult LoginSuccessful(ClaimsPrincipal principal)
//    {
//        return new Microsoft.AspNetCore.Mvc.SignInResult(
//            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
//            principal
//        );
//    }
//    private static IActionResult LoginFailed(IDictionary<string, string>? messages = null)
//    {
//        if (messages.IsNotNullOrEmpty())
//        {
//            return new ForbidResult(
//                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
//                new AuthenticationProperties(messages!)
//            );
//        }
//        return new ForbidResult(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
//    }
//    private async Task<(bool isSuccess, string? errorMessage)> ValidateSignInRequirementsAsync(
//        UserLoginInfo userLoginInfo
//    )
//    {
//        await Task.CompletedTask;
//        if (
//            _signInManager.Options.SignIn.RequireConfirmedEmail
//            && userLoginInfo.AuthenticationMethod == AuthenticationMethod.Email
//            && !userLoginInfo.IsConfirmed
//        )
//        {
//            return (false, _localizer["identity.emailNotConfirmed"]);
//        }
//        if (
//            _signInManager.Options.SignIn.RequireConfirmedPhoneNumber
//            && userLoginInfo.AuthenticationMethod == AuthenticationMethod.Username
//            && !userLoginInfo.IsConfirmed
//        )
//        {
//            return (false, _localizer["identity.phoneNumberNotConfirmed"]);
//        }
//        return (true, null);
//    }
//    private async Task<(bool isSuccess, string? errorMessage)> ValidateUserStatusAsync(User user)
//    {
//        if (!user.IsActive)
//        {
//            return (false, _localizer["identity.userNotActive"]);
//        }
//        if (_userManager.SupportsUserLockout && await _userManager.IsLockedOutAsync(user))
//        {
//            return (false, _localizer["identity.accountLockedOut"]);
//        }
//        return (true, null);
//    }
//    private async Task<(bool isSuccess, string? errorMessage)> ValidateUserPasswordAsync(
//        User user,
//        string password
//    )
//    {
//        if (!await _userManager.CheckPasswordAsync(user, password))
//        {
//            if (_userManager.SupportsUserLockout)
//            {
//                await _userManager.AccessFailedAsync(user);
//            }
//            return (false, _localizer["identity.openId.authenticationFailed"]);
//        }
//        return (true, null);
//    }
//}
