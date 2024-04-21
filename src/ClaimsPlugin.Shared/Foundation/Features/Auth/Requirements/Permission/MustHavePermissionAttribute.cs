using Microsoft.AspNetCore.Authorization;

namespace ClaimsPlugin.Shared.Foundation.Features.Auth.Requirements.Permission;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(object permission)
    {
        // if (permission is null)
        // {
        //     throw new ArgumentNullException($"The permission '{nameof(permission)}' is null.");
        // }

        // permission.GetType().ThrowExceptionIfEnumIsNotCorrect();

        // Policy = PermissionManager.GetPermissionInformation(permission).Key;
    }
}
