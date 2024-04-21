using System.Globalization;
using Foundation.Common.Persistence.Models;
using Foundation.Common.Utilities;
using Foundation.Features.ExceptionHandling.Exceptions;
using Foundation.Features.Validation.Simple;

namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

public class Amount : BaseValueObject
{
    protected Amount()
    {
    }

    private Amount(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; private set; }

    public static Amount Default => new(0);
    
    public static Amount Create(decimal value)
    {
        return new Amount(value.ToDecimalPlaces(4));
    }

    public static Amount Sum<TSource>(IEnumerable<TSource> source, Func<TSource, Amount> selector)
    {
        return source.Aggregate(Default, (current, item) => current + selector(item));
    }

    public static Amount Max(params Amount[] amounts)
    {
        AbortWhen(amounts.Length == 0, $"At least one amount is required.");

        if (amounts.Length == 1)
        {
            return amounts[0];
        }
        
        Amount highestAmount = amounts[0];
            
        for (int i = 1; i < amounts.Length; i++)
        {
            if (highestAmount < amounts[i])
            {
                highestAmount = amounts[i];
            }
        }

        return Create(highestAmount.Value);
    }
    
    public static Amount Min(params Amount[] amounts)
    {
        AbortWhen(amounts.Length == 0, $"At least one amount is required.");

        if (amounts.Length == 1)
        {
            return amounts[0];
        }
        
        Amount lowestAmount = amounts[0];
            
        for (int i = 1; i < amounts.Length; i++)
        {
            if (lowestAmount > amounts[i])
            {
                lowestAmount = amounts[i];
            }
        }

        return Create(lowestAmount.Value);
    }
    
    public static Amount operator +(Amount a, Amount b)
    {
        return Create(a.Value + b.Value);
    }

    public static Amount operator +(Amount a, decimal b)
    {
        return Create(a.Value + b);
    }

    public static Amount operator +(decimal a, Amount b)
    {
        return Create(a + b.Value);
    }

    public static Amount operator -(Amount a, Amount b)
    {
        return Create(a.Value - b.Value);
    }

    public static Amount operator -(Amount a, decimal b)
    {
        return Create(a.Value - b);
    }

    public static Amount operator -(decimal a, Amount b)
    {
        return Create(a - b.Value);
    }

    public static Amount operator *(Amount a, Amount b)
    {
        return Create(a.Value * b.Value);
    }

    public static Amount operator *(Amount a, decimal b)
    {
        return Create(a.Value * b);
    }

    public static Amount operator *(decimal a, Amount b)
    {
        return Create(a * b.Value);
    }

    public static Amount operator /(Amount a, Amount b)
    {
        return Create(a.Value / b.Value);
    }

    public static Amount operator /(Amount a, decimal b)
    {
        return Create(a.Value / b);
    }

    public static Amount operator /(decimal a, Amount b)
    {
        return Create(a / b.Value);
    }

    public static bool operator <(Amount a, decimal b)
    {
        return a.Value < b;
    }
    public static bool operator <(Amount a, Amount b)
    {
        return a.Value < b.Value;
    }
    
    public static bool operator <(decimal a, Amount b)
    {
        return a < b.Value;
    }
   
    public static bool operator >(Amount a, decimal b)
    {
        return a.Value > b;
    }
    
    public static bool operator >(Amount a, Amount b)
    {
        return a.Value > b.Value;
    }
    
    public static bool operator >(decimal a, Amount b)
    {
        return a > b.Value;
    }
    
    public static bool operator <=(Amount a, decimal b)
    {
        return a.Value <= b;
    }
    public static bool operator <=(Amount a, Amount b)
    {
        return a.Value <= b.Value;
    }
    
    public static bool operator <=(decimal a, Amount b)
    {
        return a <= b.Value;
    }
   
    public static bool operator >=(Amount a, decimal b)
    {
        return a.Value >= b;
    }
    
    public static bool operator >=(Amount a, Amount b)
    {
        return a.Value >= b.Value;
    }
    
    public static bool operator >=(decimal a, Amount b)
    {
        return a >= b.Value;
    }

    public static implicit operator decimal(Amount value)
    {
        return value.Value;
    }

    public static implicit operator string(Amount value)
    {
        return value.ToString();
    }


    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
