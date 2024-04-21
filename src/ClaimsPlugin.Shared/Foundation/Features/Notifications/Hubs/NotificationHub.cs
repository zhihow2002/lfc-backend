using ClaimsPlugin.Shared.Foundation.Features.DependencyInjection.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
namespace ClaimsPlugin.Shared.Foundation.Features.Notifications.Hubs;
[Authorize]
public class NotificationHub : Hub,
    ITransientService
{
    private readonly ILogger<NotificationHub> _logger;
    public NotificationHub(ILogger<NotificationHub> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public override async Task OnConnectedAsync()
    {
        HttpContext? context = Context.GetHttpContext();
        if (context.IsNull())
        {
            throw new InvalidOperationException("HttpContext is empty.");
        }
        ICurrentTenant? currentTenant = context.RequestServices.GetService<ICurrentTenant>();
        if (currentTenant.IsNotNull() && currentTenant.HasTenant())
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Group-{currentTenant.GetTenantId()}");
        }
        else
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Group");
        }
        await base.OnConnectedAsync();
        _logger.LogInformation("A client connected to NotificationHub: {ConnectionId}", Context.ConnectionId);
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        HttpContext? context = Context.GetHttpContext();
        if (context.IsNull())
        {
            throw new InvalidOperationException("HttpContext is empty.");
        }
        ICurrentTenant? currentTenant = context.RequestServices.GetService<ICurrentTenant>();
        if (currentTenant.IsNotNull() && currentTenant.HasTenant())
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Group-{currentTenant.GetTenantId()}");
        }
        else
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Group");
        }
        await base.OnDisconnectedAsync(exception);
        _logger.LogInformation("A client disconnected from NotificationHub: {ConnectionId}", Context.ConnectionId);
    }
}
