using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.EventSourcing.Sources.Domain.Models;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling;

namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;

public abstract class BaseEntity : BaseEntity<Guid> { }

public abstract class BaseEntity<TId> : IEntity<TId>
{
    [NotMapped]
    public List<DomainEvent> DomainEvents { get; } = new();

    [Column(Order = 0)]
    public virtual TId Id { get; protected set; } = default!;

    /// <summary>
    ///     A fail fast method to abort current action/ operation in the domain with a condition.
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="reason"></param>
    /// <exception cref="DomainException"></exception>
    protected static void AbortWhen([DoesNotReturnIf(true)] bool condition, string? reason)
    {
        if (condition)
        {
            throw new DomainException(reason);
        }
    }

    /// <summary>
    ///     A convenient method to execute an action/ operation in the domain with a condition.
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="action"></param>
    protected static void DoWhen(bool condition, Action action)
    {
        if (condition)
        {
            action();
        }
    }

    // TODO: To switch to new bottom type once C# team has introduced it.
    // Trace the discussion here https://github.com/dotnet/csharplang/issues/538.
    // Once this is included, the compiler will be able to detect unreachable codes when this function is used.
    // As for now, the 'DoesNotReturn' is a temporary solution that does removing the false nullable warning but does
    // not have strong guarantees needed to actually prevent your code from compiling if your method does actually return.
    /// <summary>
    ///     A fail fast method to abort current action/ operation in the domain.
    /// </summary>
    /// <param name="reason"></param>
    /// <exception cref="DomainException"></exception>
    [DoesNotReturn]
    protected static void Abort(string? reason)
    {
        throw new DomainException(reason);
    }
}
