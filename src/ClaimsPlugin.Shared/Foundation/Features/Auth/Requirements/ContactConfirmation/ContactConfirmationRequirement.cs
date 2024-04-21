using Microsoft.AspNetCore.Authorization;

namespace ClaimsPlugin.Shared.Foundation.Features.Auth.Requirements.ContactConfirmation;

public class ContactConfirmationRequirement : IAuthorizationRequirement
{
    public bool IsMandatory { get; }

    public ContactConfirmationRequirement(bool isMandatory)
    {
        IsMandatory = isMandatory;
    }
}
