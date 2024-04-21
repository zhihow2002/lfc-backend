using System.Security.Claims;

namespace ClaimsPlugin.Shared.Foundation.Features.Auth.Interfaces;

public interface ICurrentIdentityInitializer
{
    void SetCurrentIdentity(ClaimsPrincipal identity);

    void SetCurrentIdentityId(string identityId);
}
