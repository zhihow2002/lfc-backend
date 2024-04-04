using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Repository.Services
{
    public interface IAuditLogEventDispatcher
    {
          Task DispatchAsync(IAuditLogEvent @event, CancellationToken cancellationToken = default);
    }
}