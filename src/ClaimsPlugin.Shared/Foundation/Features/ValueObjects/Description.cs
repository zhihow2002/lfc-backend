//using Foundation.Common.Persistence.Models;
//using Foundation.Features.ExceptionHandling.Exceptions;
//using Foundation.Features.Validation.Simple;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class Description : BaseValueObject
//{
//    protected Description()
//    {
//    }

//    private Description(string value)
//    {
//        Value = value;
//    }

//    public string Value { get; private set; } = default!;

//    public static Description Create(string description)
//    {
//        if (description.IsNullOrWhiteSpace(out string? descriptionNullOrWhiteSpaceErrorMessage))
//        {
//            throw new DomainException(descriptionNullOrWhiteSpaceErrorMessage);
//        }

//        if (description.HasLengthMoreThan(500, out string? descriptionMaximumLengthErrorMessage))
//        {
//            throw new DomainException(descriptionMaximumLengthErrorMessage);
//        }

//        return new Description(description);
//    }


//    public static implicit operator string(Description description)
//    {
//        return description.ToString();
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
