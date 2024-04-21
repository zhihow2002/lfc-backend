using Foundation.Common.Persistence.Models;
using Foundation.Features.ExceptionHandling.Exceptions;
using Foundation.Features.Validation.Simple;

namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

public class QuantityWithIntegerType : BaseValueObject
{
    protected QuantityWithIntegerType()
    {
    }

    private QuantityWithIntegerType(int value)
    {
        Value = value;
    }

    public static QuantityWithIntegerType Default => new(0);
    public int Value { get; private set; }

    public static QuantityWithIntegerType Create(int quantity)
    {
        if (quantity.IsNegative(out string? quantityNegativeErrorMessage))
        {
            throw new DomainException(quantityNegativeErrorMessage);
        }

        return new QuantityWithIntegerType(quantity);
    }


    public static implicit operator int(QuantityWithIntegerType value)
    {
        return value.Value;
    }

    public static implicit operator string(QuantityWithIntegerType email)
    {
        return email.ToString();
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
