using ClaimsPlugin.Shared.Foundation.Features.EventSourcing.Sources.Domain.Models;

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
