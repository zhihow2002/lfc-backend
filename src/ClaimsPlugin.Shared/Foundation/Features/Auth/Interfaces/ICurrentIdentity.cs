using System.Security.Claims;
using ClaimsPlugin.Shared.Foundation.Features.Auth.Models;

namespace ClaimsPlugin.Shared.Foundation.Features.Auth.Interfaces;

public interface ICurrentIdentity
{
    Guid GetIdentityId();
    string GetIdentityName();
    List<Claim>? GetIdentityClaims(string type);
    IdentityType GetIdentityType();
}
