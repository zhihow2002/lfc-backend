
using Ardalis.Specification;
using ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.EventSourcing.Sources.Domain.Events;


namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Decorators;

/// <summary>
///     The repository that implements IRepositoryWithEvents.
///     Implemented as a decorator. It only augments the Add,
///     Update and Delete calls where it adds the respective
///     EntityCreated, EntityUpdated or EntityDeleted event
///     before delegating to the decorated repository.
/// </summary>
public class EventAddingRepositoryDecorator<T> : IRepositoryWithEvents<T>
    where T : class, IAggregateRoot
{
    private readonly IRepository<T> _decorated;

    public EventAddingRepositoryDecorator(IRepository<T> decorated)
    {
        _decorated = decorated;
    }

    public Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.DomainEvents.Add(EntityCreatedDomainEvent.WithEntity(entity));
        return _decorated.AddAsync(entity, cancellationToken);
    }

    public Task<IEnumerable<T>> AddRangeAsync(
        IEnumerable<T> entities,
        CancellationToken cancellationToken = default
    )
    {
        foreach (T entity in entities)
        {
            entity.DomainEvents.Add(EntityCreatedDomainEvent.WithEntity(entity));
        }

        return _decorated.AddRangeAsync(entities, cancellationToken);
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.DomainEvents.Add(EntityUpdatedDomainEvent.WithEntity(entity));

        return _decorated.UpdateAsync(entity, cancellationToken);
    }

    Task IRepositoryBase<T>.UpdateRangeAsync(
        IEnumerable<T> entities,
        CancellationToken cancellationToken
    )
    {
        return UpdateRangeAsync(entities, cancellationToken);
    }

    public Task UpdateRangeAsync(
        IEnumerable<T> entities,
        CancellationToken cancellationToken = default
    )
    {
        foreach (T entity in entities)
        {
            entity.DomainEvents.Add(EntityUpdatedDomainEvent.WithEntity(entity));
        }

        return _decorated.UpdateRangeAsync(entities, cancellationToken);
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.DomainEvents.Add(EntityDeletedDomainEvent.WithEntity(entity));
        return _decorated.DeleteAsync(entity, cancellationToken);
    }

    public Task DeleteRangeAsync(
        IEnumerable<T> entities,
        CancellationToken cancellationToken = default
    )
    {
        foreach (T entity in entities)
        {
            entity.DomainEvents.Add(EntityDeletedDomainEvent.WithEntity(entity));
        }

        return _decorated.DeleteRangeAsync(entities, cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _decorated.SaveChangesAsync(cancellationToken);
    }

    public Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
        where TId : notnull
    {
        return _decorated.GetByIdAsync(id, cancellationToken);
    }

    [Obsolete]
    public Task<T?> GetBySpecAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default
    )
    {
        return _decorated.GetBySpecAsync(specification, cancellationToken);
    }

    [Obsolete]
    public Task<TResult?> GetBySpecAsync<TResult>(
        ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = default
    )
    {
        return _decorated.GetBySpecAsync(specification, cancellationToken);
    }

    public Task<T?> FirstOrDefaultAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default
    )
    {
        return _decorated.FirstOrDefaultAsync(specification, cancellationToken);
    }

    public Task<TResult?> FirstOrDefaultAsync<TResult>(
        ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = default
    )
    {
        return _decorated.FirstOrDefaultAsync(specification, cancellationToken);
    }

    public Task<T?> SingleOrDefaultAsync(
        ISingleResultSpecification<T> specification,
        CancellationToken cancellationToken = default
    )
    {
        return _decorated.SingleOrDefaultAsync(specification, cancellationToken);
    }

    public Task<TResult?> SingleOrDefaultAsync<TResult>(
        ISingleResultSpecification<T, TResult> specification,
        CancellationToken cancellationToken = default
    )
    {
        return _decorated.SingleOrDefaultAsync(specification, cancellationToken);
    }

    public Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        return _decorated.ListAsync(cancellationToken);
    }

    public Task<List<T>> ListAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default
    )
    {
        return _decorated.ListAsync(specification, cancellationToken);
    }

    public Task<List<TResult>> ListAsync<TResult>(
        ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = default
    )
    {
        return _decorated.ListAsync(specification, cancellationToken);
    }

    public Task<bool> AnyAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default
    )
    {
        return _decorated.AnyAsync(specification, cancellationToken);
    }

    public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return _decorated.AnyAsync(cancellationToken);
    }

    public Task<int> CountAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default
    )
    {
        return _decorated.CountAsync(specification, cancellationToken);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return _decorated.CountAsync(cancellationToken);
    }

    public Task DeleteRangeAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<T> AsAsyncEnumerable(ISpecification<T> specification)
    {
        throw new NotImplementedException();
    }
}
