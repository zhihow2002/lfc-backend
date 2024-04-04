using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClaimsPlugin.Service

namespace ClaimsPlugin.Shared.Service
{
    internal class AuditLogEventDispatcher 
    {
         private readonly IMediator _mediator;

        public AuditLogEventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task DispatchAsync(IAuditLogEvent @event, CancellationToken cancellationToken = default)
        {
            await _mediator.Publish(@event, cancellationToken);
        }
        
    }
}