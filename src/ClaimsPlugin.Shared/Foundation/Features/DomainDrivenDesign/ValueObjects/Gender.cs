using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;

namespace ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.ValueObjects;

public class Gender : BaseValueObject
{
    protected Gender() { }

    private Gender(string value)
    {
        Value = value;
    }

    public static Gender Male => new(nameof(Male));
    public static Gender Female => new(nameof(Female));
    public static Gender Other => new(nameof(Other));
    public string Value { get; private set; } = default!;

    public static IEnumerable<Gender> SupportedItems
    {
        get
        {
            yield return Male;
            yield return Female;
            yield return Other;
        }
    }

    public static Gender From(string value)
    {
        Gender item = new(value);

        if (!SupportedItems.Contains(item))
        {
            throw new DomainException($"Unsupported {nameof(Gender)}: {value}");
        }

        return item;
    }

    public static implicit operator string(Gender value)
    {
        return value.ToString();
    }

    public static explicit operator Gender(string value)
    {
        return From(value);
    }

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
