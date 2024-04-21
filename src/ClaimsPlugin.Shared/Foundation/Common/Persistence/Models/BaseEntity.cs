using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.EventSourcing.Sources.Domain.Models;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;
namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
public abstract class BaseEntity : BaseEntity<Guid> { }
public abstract class BaseEntity<TId> : IEntity<TId>
{
    [NotMapped]
    public List<DomainEvent> DomainEvents { get; } = new();
    [Column(Order = 0)]
    public virtual TId Id { get; protected set; } = default!;
    protected static void AbortWhen([DoesNotReturnIf(true)] bool condition, string? reason)
    {
        if (condition)
        {
            throw new DomainException(reason);
        }
    }
    protected static void DoWhen(bool condition, Action action)
    {
        if (condition)
        {
            action();
        }
    }
    [DoesNotReturn]
    protected static void Abort(string? reason)
    {
        throw new DomainException(reason);
    }
}
