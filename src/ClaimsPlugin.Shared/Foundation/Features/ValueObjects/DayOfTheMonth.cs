//using Foundation.Common.Persistence.Models;
//using Foundation.Features.ExceptionHandling.Exceptions;
//using Foundation.Features.Validation.Simple;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class DayOfTheMonth : BaseValueObject
//{
//    protected DayOfTheMonth()
//    {
//    }

//    private DayOfTheMonth(int value)
//    {
//        Value = value;
//    }

//    public static DayOfTheMonth Default => new(1);
//    public int Value { get; private set; }

//    public static DayOfTheMonth Create(int value)
//    {
//        if (value.IsNotBetween(1, 31, out string? valueNotBetweenErrorMessage))
//        {
//            throw new DomainException(valueNotBetweenErrorMessage);
//        }

//        return new DayOfTheMonth(value);
//    }


//    public static implicit operator int(DayOfTheMonth value)
//    {
//        return value.Value;
//    }

//    public static implicit operator string(DayOfTheMonth value)
//    {
//        return value.ToString();
//    }


//    public override string ToString()
//    {
//        return Value.ToString();
//    }

//    protected override IEnumerable<object> GetEqualityComponents()
//    {
//        yield return Value;
//    }
//}
