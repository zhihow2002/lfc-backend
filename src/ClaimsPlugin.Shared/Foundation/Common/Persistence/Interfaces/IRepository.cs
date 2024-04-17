using Ardalis.Specification;
using ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.Interfaces;

namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;

/// <summary>
///     The regular read/write repository for an aggregate root.
/// </summary>
public interface IRepository<T> : IRepositoryBase<T>
    where T : class, IAggregateRoot { }
