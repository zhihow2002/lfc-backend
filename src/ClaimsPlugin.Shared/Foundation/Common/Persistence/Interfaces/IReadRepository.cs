using Ardalis.Specification;
using ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.Interfaces;
namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;

/// <summary>
///     The read-only repository for an aggregate root.
/// </summary>
public interface IReadRepository<T> : IReadRepositoryBase<T>
    where T : class, IAggregateRoot { }
