using Microsoft.AspNetCore.Authorization;

namespace ClaimsPlugin.Shared.Foundation.Features.Auth.Requirements.PasswordReset;

public class PasswordResetRequirement : IAuthorizationRequirement
{
    public bool IsMandatory { get; }

    public PasswordResetRequirement(bool isMandatory)
    {
        IsMandatory = isMandatory;
    }
}
