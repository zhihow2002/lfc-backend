using Mediator;
using Microsoft.AspNetCore.Http;
using Audit.Core;
using ClaimsPlugin.Domain.Events;

namespace ClaimsPlugin.Repository.AuditLogEventHandler.cs
{
     public class AuditLogEventHandler : INotificationHandler<AuditLogEvent>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditLogEventHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async ValueTask Handle(AuditLogEvent notification, CancellationToken cancellationToken)
        {
            using (var audit = await AuditScope.CreateAsync(notification.EventType, () => notification.Data))
            {
                audit.SetCustomField("ReferenceId", notification.ReferenceId);
                audit.SetCustomField("IpAddress", _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
                audit.Comment(notification.Message);
            }
        }
    }
}