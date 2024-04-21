using Foundation.Common.Persistence.Models;
using Foundation.Features.ExceptionHandling.Exceptions;

namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

public class UnitOfLength : BaseValueObject
{
    protected UnitOfLength()
    {
    }

    private UnitOfLength(decimal value)
    {
        Value = value;
    }

    public static UnitOfLength Kilometers => new(1.609344M);
    public static UnitOfLength NauticalMiles => new(0.8684M);
    public static UnitOfLength Miles => new(1M);

    public decimal Value { get; private set; } = default!;

    public decimal ConvertFromMiles(decimal input)
    {
        return input * Value;
    }
    
    public decimal ConvertFromMiles(double input)
    {
        return Convert.ToDecimal(input) * Value;
    }

    public decimal ConvertFromDegrees(double input)
    {
        return Convert.ToDecimal(input) * 60m * 1.1515m * Value;
    }

    public decimal ConvertToDegrees(double input)
    {
        return Convert.ToDecimal(input) / 60m / 1.1515m / Value;
    }
    
    public decimal ConvertToDegrees(decimal input)
    {
        return ConvertToDegrees(Convert.ToDouble(input));
    }
    
    public static IEnumerable<UnitOfLength> SupportedItems
    {
        get
        {
            yield return Kilometers;
            yield return NauticalMiles;
            yield return Miles;
        }
    }
    
    public static UnitOfLength From(decimal value)
    {
        UnitOfLength item = new(value);

        if (!SupportedItems.Contains(item))
        {
            throw new DomainException($"Unsupported {nameof(UnitOfLength)}: {value}");
        }

        return item;
    }

    public static implicit operator string(UnitOfLength value)
    {
        return value.ToString();
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
