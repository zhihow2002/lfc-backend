using Microsoft.AspNetCore.SignalR;
using OpenIddict.Abstractions;

namespace ClaimsPlugin.Shared.Foundation.Features.Notifications.Providers;

public class UserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User.FindFirst(OpenIddictConstants.Claims.Subject)?.Value;
    }
}
