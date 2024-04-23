//using Foundation.Common.Persistence.Models;
//using Foundation.Features.ExceptionHandling.Exceptions;
//using Foundation.Features.Validation.Simple;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class Duration : BaseValueObject
//{
//    protected Duration()
//    {
//    }

//    private Duration(DurationType type, int value)
//    {
//        Type = type;
//        Value = value;
//    }

//    public DurationType Type { get; private set; } = default!;
//    public int Value { get; private set; }

//    public Duration Duplicate()
//    {
//        return Create(DurationType.From(Type.Value), Value);
//    }

//    public static Duration Create(DurationType type, int value)
//    {
//        if (value.IsNegative(out string? countedDaysNegativeErrormessage))
//        {
//            throw new DomainException(countedDaysNegativeErrormessage);
//        }

//        return new Duration(type, value);
//    }

//    public static implicit operator string(Duration duration)
//    {
//        return duration.ToString();
//    }

//    public override string ToString()
//    {
//        return $"{Value} {Type}(s)";
//    }

//    protected override IEnumerable<object?> GetEqualityComponents()
//    {
//        yield return Type;
//        yield return Value;
//    }
//}
