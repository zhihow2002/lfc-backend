using System.Diagnostics.CodeAnalysis;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;
namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
[Serializable]
public abstract class BaseValueObject : IComparable,
    IComparable<BaseValueObject>
{
    public static bool operator ==(BaseValueObject? a, BaseValueObject? b)
    {
        if (a is null && b is null)
        {
            return true;
        }
        if (a is null || b is null)
        {
            return false;
        }
        return a.Equals(b);
    }
    public static bool operator !=(BaseValueObject a, BaseValueObject b)
    {
        return !(a == b);
    }
    public virtual int CompareTo(object? obj)
    {
        Type? thisType = GetRealType(this);
        Type? otherType = GetRealType(obj);
        if (thisType is null)
        {
            throw new InternalServerException("Can't get the source type in value object.");
        }
        if (otherType is null)
        {
            throw new InternalServerException("Can't get the target type in value object.");
        }
        if (thisType != otherType)
        {
            return string.Compare(thisType.ToString(), otherType.ToString(), StringComparison.Ordinal);
        }
        BaseValueObject? other = (BaseValueObject?)obj;
        object?[] components = GetEqualityComponents().ToArray();
        object?[] otherComponents = other!.GetEqualityComponents().ToArray();
        return components.Select((t, i) => CompareComponents(t, otherComponents[i])).FirstOrDefault(comparison => comparison != 0);
    }
    public virtual int CompareTo(BaseValueObject? other)
    {
        return CompareTo(other as object);
    }
    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }
        Type? sourceType = GetRealType(this);
        Type? targetType = GetRealType(obj);
        if (sourceType != targetType)
        {
            throw new InternalServerException($"Can't compare the source type '{sourceType?.Name}' with target type '{targetType?.Name}'.");
        }
        BaseValueObject valueObject = (BaseValueObject)obj;
        return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
    }
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(
                1,
                (current, obj) =>
                {
                    unchecked
                    {
                        return current * 23 + (obj?.GetHashCode() ?? 0);
                    }
                }
            );
    }
    protected static void AbortWhen([DoesNotReturnIf(true)] bool condition, string reason)
    {
        if (condition)
        {
            throw new DomainException(reason);
        }
    }
    [DoesNotReturn]
    protected static void Abort(string reason)
    {
        throw new DomainException(reason);
    }
    protected abstract IEnumerable<object?> GetEqualityComponents();
    private static Type? GetRealType(object? obj)
    {
        const string efCoreProxyPrefix = "Castle.Proxies.";
        Type? type = obj?.GetType();
        string? typeString = type?.ToString();
        return typeString != null && typeString.Contains(efCoreProxyPrefix) ? type?.BaseType : type;
    }
    private static int CompareComponents(object? object1, object? object2)
    {
        if (object1 is null && object2 is null)
        {
            return 0;
        }
        if (object1 is null)
        {
            return -1;
        }
        if (object2 is null)
        {
            return 1;
        }
        if (object1 is IComparable comparable1 && object2 is IComparable comparable2)
        {
            return comparable1.CompareTo(comparable2);
        }
        return object1.Equals(object2) ? 0 : -1;
    }
}
