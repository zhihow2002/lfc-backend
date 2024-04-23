//using Foundation.Common.Persistence.Models;
//using Foundation.Features.ExceptionHandling.Exceptions;
//using Foundation.Features.Validation.Simple;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class UniqueIdentityNumber : BaseValueObject
//{
//    protected UniqueIdentityNumber()
//    {
//    }

//    private UniqueIdentityNumber(string value)
//    {
//        Value = value;
//    }

//    public string Value { get; private set; } = default!;

//    public static UniqueIdentityNumber Create(string value)
//    {
//        if (value.IsNullOrWhiteSpace(out string? valueNullOrWhiteSpaceErrorMessage))
//        {
//            throw new DomainException(valueNullOrWhiteSpaceErrorMessage);
//        }

//        if (value.HasLengthMoreThan(50, out string? valueMaximumLengthErrorMessage))
//        {
//            throw new DomainException(valueMaximumLengthErrorMessage);
//        }
        
//        // TODO: to verify based on different country

//        return new UniqueIdentityNumber(value);
//    }

//    public static implicit operator string(UniqueIdentityNumber email)
//    {
//        return email.ToString();
//    }


//    public override string ToString()
//    {
//        return Value;
//    }


//    protected override IEnumerable<object> GetEqualityComponents()
//    {
//        yield return Value;
//    }
//}
