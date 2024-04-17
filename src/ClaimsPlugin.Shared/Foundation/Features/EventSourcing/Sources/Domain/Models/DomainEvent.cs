using ClaimsPlugin.Shared.Foundation.Features.EventSourcing.Sources.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Shared.Foundation.Features.EventSourcing.Sources.Domain.Models;

public abstract class DomainEvent : IDomainEvent
{
    public DateTime TriggeredOn { get; protected set; } = DateTime.Now;
}
