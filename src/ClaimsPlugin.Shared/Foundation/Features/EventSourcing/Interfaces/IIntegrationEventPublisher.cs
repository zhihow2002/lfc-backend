using ClaimsPlugin.Shared.Foundation.Features.EventSourcing.Sources.Intergration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Shared.Foundation.Features.EventSourcing.Interfaces
{
    public interface IIntegrationEventPublisher
    {
         Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class, IIntegrationEvent;
    }
}