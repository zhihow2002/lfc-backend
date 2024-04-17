using ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.EventSourcing.Sources.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Shared.Foundation.Features.EventSourcing.Sources.Domain.Events;

// Generic domain events that are published automatically.
public static class EntityUpdatedDomainEvent
{
    public static EntityUpdatedDomainEvent<TEntity> WithEntity<TEntity>(TEntity entity)
        where TEntity : IEntity
    {
        return new EntityUpdatedDomainEvent<TEntity>(entity);
    }
}

// Generic domain events that are published automatically.
public class EntityUpdatedDomainEvent<TEntity> : DomainEvent
    where TEntity : IEntity
{
    internal EntityUpdatedDomainEvent(TEntity entity)
    {
        Entity = entity;
    }

    public TEntity Entity { get; }
}
