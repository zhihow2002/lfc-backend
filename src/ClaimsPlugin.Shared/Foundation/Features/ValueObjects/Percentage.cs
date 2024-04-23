//using Foundation.Common.Persistence.Models;
//using Foundation.Common.Utilities;
//using Foundation.Features.ExceptionHandling.Exceptions;
//using Foundation.Features.Validation.Simple;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class Percentage : BaseValueObject
//{
//    protected Percentage()
//    {
//    }

//    private Percentage(decimal value)
//    {
//        Value = value;
//    }

//    public decimal Value { get; private set; }

//    public static Percentage Default => new(0);
//    public static Percentage Create(decimal value)
//    {
//        if (value.IsNegative(out string? valueNegativeErrormessage))
//        {
//            throw new DomainException(valueNegativeErrormessage);
//        }

//        return new Percentage(value.ToDecimalPlaces(2));
//    }

//    /// <summary>
//    ///     Get the percentage of the <paramref name="part"/> is of <paramref name="whole"/>
//    /// </summary>
//    /// <param name="whole"></param>
//    /// <param name="part"></param>
//    /// <returns></returns>
//    public static Percentage Create(decimal whole, decimal part)
//    {
//        return new Percentage((part / whole).ToDecimalPlaces(2));
//    }
    
//    /// <summary>
//    ///     Get the total value with the given <paramref name="portion" /> and <see cref="Percentage" />.
//    /// </summary>
//    /// <param name="portion">Total value = portion ÷ (percentage ÷ 100)</param>
//    /// <returns></returns>
//    public decimal GetTotalValueWithGivenPortion(decimal portion)
//    {
//        if (Value == 0)
//        {
//            return portion;
//        }
        
//        return portion / (1 + (Value / 100));
//    }

//    /// <summary>
//    ///     Get the portion with the given <paramref name="totalValue" /> and <see cref="Percentage" />.
//    /// </summary>
//    /// <param name="totalValue">Portion = totalValue × (percentage ÷ 100)</param>
//    /// <returns></returns>
//    public decimal GetPortionWithGivenTotalValue(decimal totalValue)
//    {
//        return totalValue * (Value / 100);
//    }

//    public static implicit operator decimal(Percentage value)
//    {
//        return value.Value;
//    }

//    public static implicit operator string(Percentage value)
//    {
//        return value.ToString();
//    }

//    public override string ToString()
//    {
//        return $"{Value}%";
//    }

//    protected override IEnumerable<object> GetEqualityComponents()
//    {
//        yield return Value;
//    }
//}
