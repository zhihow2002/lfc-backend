using ClaimsPlugin.Shared.Foundation.Features.EventSourcing.Sources.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Shared.Foundation.Features.EventSourcing.Interfaces
{
    public interface IDomainEventPublisher
    {
         Task PublishAsync(IDomainEvent @event, CancellationToken cancellationToken = default);
    }
}