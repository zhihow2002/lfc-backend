//using Foundation.Common.Persistence.Models;
//using Foundation.Features.ExceptionHandling.Exceptions;
//using Foundation.Features.Validation.Simple;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class WebAddress : BaseValueObject
//{
//    protected WebAddress()
//    {
//    }

//    private WebAddress(string value)
//    {
//        Value = value;
//    }

//    public string Value { get; private set; } = default!;

//    public static WebAddress Create(string website)
//    {
//        if (website.IsNullOrWhiteSpace(out string? websiteNullOrWhiteSpaceErrorMessage))
//        {
//            throw new DomainException(websiteNullOrWhiteSpaceErrorMessage);
//        }

//        if (website.HasLengthMoreThan(500, out string? websiteMaximumLengthErrorMessage))
//        {
//            throw new DomainException(websiteMaximumLengthErrorMessage);
//        }

//        if (website.IsNotWebAddress(out string? websiteNotWebAddressErrorMessage))
//        {
//            throw new DomainException(websiteNotWebAddressErrorMessage);
//        }

//        return new WebAddress(website);
//    }

//    public static implicit operator string(WebAddress webAddress)
//    {
//        return webAddress.ToString();
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
