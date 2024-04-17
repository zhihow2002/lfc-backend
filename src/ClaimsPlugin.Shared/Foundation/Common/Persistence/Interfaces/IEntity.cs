using ClaimsPlugin.Shared.Foundation.Features.EventSourcing.Sources.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces
{
    public interface IEntity
    {
        List<DomainEvent> DomainEvents { get; }
    }

    public interface IEntity<TId> : IEntity
    {
        TId Id { get; }
    }
}
