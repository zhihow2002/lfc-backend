//using System.Globalization;
//using Foundation.Common.Persistence.Models;
//using Foundation.Common.Utilities;
//using Foundation.Features.ExceptionHandling.Exceptions;
//using Foundation.Features.Validation.Simple;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class CurrencyExchangeRate : BaseValueObject
//{
//    protected CurrencyExchangeRate()
//    {
//    }

//    private CurrencyExchangeRate(decimal value)
//    {
//        Value = value;
//    }

//    public static CurrencyExchangeRate Default => new(1);

//    public decimal Value { get; private set; }

//    public static CurrencyExchangeRate Create(decimal value)
//    {
//        if (value.IsNegative(out string? valueNegativeErrormessage))
//        {
//            throw new DomainException(valueNegativeErrormessage);
//        }

//        return new CurrencyExchangeRate(value.ToDecimalPlaces(10));
//    }


//    public static implicit operator decimal(CurrencyExchangeRate value)
//    {
//        return value.Value;
//    }

//    public static implicit operator string(CurrencyExchangeRate value)
//    {
//        return value.ToString();
//    }


//    public override string ToString()
//    {
//        return Value.ToString(CultureInfo.InvariantCulture);
//    }

//    protected override IEnumerable<object> GetEqualityComponents()
//    {
//        yield return Value;
//    }
//}
