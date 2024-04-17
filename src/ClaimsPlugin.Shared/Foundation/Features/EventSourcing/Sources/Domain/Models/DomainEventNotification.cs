using ClaimsPlugin.Shared.Foundation.Features.EventSourcing.Sources.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Shared.Foundation.Features.EventSourcing.Sources.Domain.Models;

public class DomainEventNotification<TDomainEvent> : INotification
    where TDomainEvent : IDomainEvent
{
    public DomainEventNotification(TDomainEvent @event)
    {
        Event = @event;
    }

    public TDomainEvent Event { get; }
}
