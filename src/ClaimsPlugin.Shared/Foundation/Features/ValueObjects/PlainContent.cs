//using Foundation.Common.Persistence.Models;
//using Foundation.Features.ExceptionHandling.Exceptions;
//using Foundation.Features.Validation.Simple;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class PlainContent : BaseValueObject
//{
//    protected PlainContent()
//    {
//    }

//    private PlainContent(string value)
//    {
//        Value = value;
//    }

//    public string Value { get; private set; } = default!;

//    public static PlainContent Create(string plainContent)
//    {
//        if (plainContent.IsNullOrWhiteSpace(out string? plainContentNullOrWhiteSpaceErrorMessage))
//        {
//            throw new DomainException(plainContentNullOrWhiteSpaceErrorMessage);
//        }

//        if (plainContent.HasLengthMoreThan(3000, out string? plainContentMaximumLengthErrorMessage))
//        {
//            throw new DomainException(plainContentMaximumLengthErrorMessage);
//        }

//        if (plainContent.IsNotRegex("/^[\\w!@#$%^&*()_-\\+=:;'\"|\\/,.<>? ]*$/"))
//        {
//            throw new DomainException("Content accepts only alphabets [a-z, A-Z], numbers [0-9], spaces, and characters [!@#$%^&*()_-+=:;'\"|/,.<>?].");
//        }

//        return new PlainContent(plainContent);
//    }


//    public static implicit operator string(PlainContent plainContent)
//    {
//        return plainContent.ToString();
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
