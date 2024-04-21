﻿using ClaimsPlugin.Shared.Foundation.Features.DependencyInjection.Interfaces;

namespace Foundation.Features.Notifications.Interfaces;

public interface INotificationSender : ITransientService
{
    Task BroadcastAsync(INotificationMessage notification, CancellationToken cancellationToken = default);
    Task BroadcastAsync(INotificationMessage notification, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken = default);

    Task SendToAllAsync(INotificationMessage notification, CancellationToken cancellationToken = default);
    Task SendToAllAsync(INotificationMessage notification, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken = default);
    Task SendToGroupAsync(INotificationMessage notification, string group, CancellationToken cancellationToken = default);
    Task SendToGroupAsync(INotificationMessage notification, string group, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken = default);
    Task SendToGroupsAsync(INotificationMessage notification, IEnumerable<string> groupNames, CancellationToken cancellationToken = default);
    Task SendToUserAsync(INotificationMessage notification, string userId, CancellationToken cancellationToken = default);
    Task SendToUserAsync(INotificationMessage notification, Guid userId, CancellationToken cancellationToken = default);
    Task SendToUsersAsync(INotificationMessage notification, IEnumerable<string> userIds, CancellationToken cancellationToken = default);
    Task SendToUsersAsync(INotificationMessage notification, IEnumerable<Guid> userIds, CancellationToken cancellationToken = default);
}
