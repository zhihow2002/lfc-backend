using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

namespace ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.ValueObjects;

public class CountedValues : BaseValueObject
{
    protected CountedValues() { }

    private CountedValues(int value)
    {
        Value = value;
    }

    public static CountedValues Default => new(0);
    public int Value { get; private set; }

    public static CountedValues Create(int value)
    {
        if (value.IsNegative(out string? countedDaysNegativeErrormessage))
        {
            throw new DomainException(countedDaysNegativeErrormessage);
        }

        return new CountedValues(value);
    }

    public static CountedValues Sum<TSource>(
        IEnumerable<TSource> source,
        Func<TSource, CountedValues> selector
    )
    {
        return source.Aggregate(Default, (current, item) => current + selector(item));
    }

    public static CountedValues Max(params CountedValues[] values)
    {
        AbortWhen(values.Length == 0, $"At least one value is required.");

        if (values.Length == 1)
        {
            return values[0];
        }

        CountedValues highestValue = values[0];

        for (int i = 1; i < values.Length; i++)
        {
            if (highestValue < values[i])
            {
                highestValue = values[i];
            }
        }

        return Create(highestValue);
    }

    public static CountedValues Min(params CountedValues[] values)
    {
        AbortWhen(values.Length == 0, $"At least one value is required.");

        if (values.Length == 1)
        {
            return values[0];
        }

        CountedValues lowestValue = values[0];

        for (int i = 1; i < values.Length; i++)
        {
            if (lowestValue > values[i])
            {
                lowestValue = values[i];
            }
        }

        return Create(lowestValue);
    }

    public static CountedValues operator +(CountedValues a, CountedValues b)
    {
        return Create(a.Value + b.Value);
    }

    public static CountedValues operator +(CountedValues a, int b)
    {
        return Create(a.Value + b);
    }

    public static CountedValues operator +(int a, CountedValues b)
    {
        return Create(a + b.Value);
    }

    public static CountedValues operator -(CountedValues a, CountedValues b)
    {
        return Create(a.Value - b.Value);
    }

    public static CountedValues operator -(CountedValues a, int b)
    {
        return Create(a.Value + b);
    }

    public static CountedValues operator -(int a, CountedValues b)
    {
        return Create(a + b.Value);
    }

    public static CountedValues operator *(CountedValues a, CountedValues b)
    {
        return Create(a.Value * a.Value);
    }

    public static CountedValues operator *(CountedValues a, int b)
    {
        return Create(a.Value * b);
    }

    public static CountedValues operator *(int a, CountedValues b)
    {
        return Create(a * b.Value);
    }

    public static CountedValues operator /(CountedValues a, CountedValues b)
    {
        return Create(a.Value * b.Value);
    }

    public static CountedValues operator /(CountedValues a, int b)
    {
        return Create(a.Value / b);
    }

    public static CountedValues operator /(int a, CountedValues b)
    {
        return Create(a / b.Value);
    }

    public static bool operator <(CountedValues a, int b)
    {
        return a.Value < b;
    }

    public static bool operator <(CountedValues a, CountedValues b)
    {
        return a.Value < b.Value;
    }

    public static bool operator <(int a, CountedValues b)
    {
        return a < b.Value;
    }

    public static bool operator >(CountedValues a, int b)
    {
        return a.Value > b;
    }

    public static bool operator >(CountedValues a, CountedValues b)
    {
        return a.Value > b.Value;
    }

    public static bool operator >(int a, CountedValues b)
    {
        return a > b.Value;
    }

    public static bool operator <=(CountedValues a, int b)
    {
        return a.Value <= b;
    }

    public static bool operator <=(CountedValues a, CountedValues b)
    {
        return a.Value <= b.Value;
    }

    public static bool operator <=(int a, CountedValues b)
    {
        return a <= b.Value;
    }

    public static bool operator >=(CountedValues a, int b)
    {
        return a.Value >= b;
    }

    public static bool operator >=(CountedValues a, CountedValues b)
    {
        return a.Value >= b.Value;
    }

    public static bool operator >=(int a, CountedValues b)
    {
        return a >= b.Value;
    }

    public static implicit operator int(CountedValues value)
    {
        return value.Value;
    }

    public static implicit operator string(CountedValues value)
    {
        return value.ToString();
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
