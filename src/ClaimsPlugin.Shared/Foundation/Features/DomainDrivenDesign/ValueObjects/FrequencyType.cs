using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;

namespace ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.ValueObjects;

public class FrequencyType : BaseValueObject
{
    protected FrequencyType() { }

    private FrequencyType(string value)
    {
        Value = value;
    }

    public static FrequencyType Daily => new(nameof(Daily));
    public static FrequencyType Weekly => new(nameof(Weekly));
    public static FrequencyType Monthly => new(nameof(Monthly));
    public static FrequencyType Yearly => new(nameof(Yearly));

    public string Value { get; private set; } = default!;

    public static IEnumerable<FrequencyType> SupportedItems
    {
        get
        {
            yield return Daily;
            yield return Weekly;
            yield return Monthly;
            yield return Yearly;
        }
    }

    public static FrequencyType From(string value)
    {
        FrequencyType item = new(value);

        if (!SupportedItems.Contains(item))
        {
            throw new DomainException($"Unsupported {nameof(FrequencyType)}: {value}");
        }

        return item;
    }

    public static implicit operator string(FrequencyType value)
    {
        return value.ToString();
    }

    public static explicit operator FrequencyType(string value)
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
