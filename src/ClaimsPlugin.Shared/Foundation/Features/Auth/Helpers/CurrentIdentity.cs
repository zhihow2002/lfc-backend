using System.Security.Claims;
using ClaimsPlugin.Shared.Foundation.Features.Auth.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.Auth.Models;
using Foundation.Features.Identity.Extensions;

namespace ClaimsPlugin.Shared.Foundation.Features.Auth.Helpers;

// Although we have signed our Jwt and implemented short-lived token, however it is still not recommended to validate/ process based on the role/ permissions on token.
// Hence this class is mainly to get the user Id from the claims.
// However, things may change in some days so feel free to add role/permissions information to the claims.
public class CurrentIdentity : ICurrentIdentity, ICurrentIdentityInitializer
{
    private ClaimsPrincipal? _identity;

    private Guid _identityId = Guid.Empty;

    public Guid GetIdentityId()
    {
        return IsAuthenticated()
            ? Guid.Parse(_identity?.GetIdentityId() ?? Guid.Empty.ToString())
            : _identityId;
    }

    public List<Claim>? GetIdentityClaims(string type)
    {
        return IsAuthenticated() ? _identity?.GetIdentityClaims(type) ?? null : null;
    }

    public string GetIdentityName()
    {
        return IsAuthenticated() ? _identity?.GetIdentityName() ?? "Anonymous" : "Anonymous";
    }

    public IdentityType GetIdentityType()
    {
        if (
            IsAuthenticated()
            && Enum.TryParse(_identity?.GetIdentityType(), true, out IdentityType valueParsed)
        )
        {
            return valueParsed;
        }

        return IdentityType.Unknown;
    }

    private bool IsAuthenticated()
    {
        return _identity?.Identity?.IsAuthenticated is true;
    }

    public void SetCurrentIdentity(ClaimsPrincipal identity)
    {
        if (_identity != null)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        _identity = identity;
    }

    public void SetCurrentIdentityId(string identityId)
    {
        if (_identityId != Guid.Empty)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        if (!string.IsNullOrEmpty(identityId))
        {
            _identityId = Guid.Parse(identityId);
        }
    }
}
