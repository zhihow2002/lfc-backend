using ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.EventSourcing.Sources.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Shared.Foundation.Features.EventSourcing.Sources.Domain.Events;

public static class EntityCreatedDomainEvent
{
    public static EntityCreatedDomainEvent<TEntity> WithEntity<TEntity>(TEntity entity)
        where TEntity : IEntity
    {
        return new EntityCreatedDomainEvent<TEntity>(entity);
    }
}

// Generic domain events that are published automatically.
public class EntityCreatedDomainEvent<TEntity> : DomainEvent
    where TEntity : IEntity
{
    internal EntityCreatedDomainEvent(TEntity entity)
    {
        Entity = entity;
    }

    public TEntity Entity { get; }
}
