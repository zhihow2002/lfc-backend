//using Foundation.Common.Persistence.Models;
//using Foundation.Features.ExceptionHandling.Exceptions;
//using Foundation.Features.Validation.Simple;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class PhoneNumber : BaseValueObject
//{
//    protected PhoneNumber()
//    {
//    }

//    private PhoneNumber(string countryCode, string digits)
//    {
//        CountryCode = countryCode;
//        Digits = digits;
//    }

//    public string FormattedNumber => $"+{UnformattedNumber}";
//    public string UnformattedNumber => $"{CountryCode}{Digits}";

//    public string CountryCode { get; private set; } = default!;
//    public string Digits { get; private set; } = default!;

//    public static PhoneNumber Create(string countryCode, string digits)
//    {
//        if (countryCode.IsNullOrWhiteSpace(out string? countryCodeNullOrWhiteSpaceErrorMessage))
//        {
//            throw new DomainException(countryCodeNullOrWhiteSpaceErrorMessage);
//        }

//        if (countryCode.HasLengthMoreThan(5, out string? countryCodeMaximumLengthErrorMessage))
//        {
//            throw new DomainException(countryCodeMaximumLengthErrorMessage);
//        }

//        if (digits.IsNullOrWhiteSpace(out string? digitsNullOrWhiteSpaceErrorMessage))
//        {
//            throw new DomainException(digitsNullOrWhiteSpaceErrorMessage);
//        }

//        if (digits.HasLengthLessThan(5, out string? digitsMinimumLengthErrorMessage))
//        {
//            throw new DomainException(digitsMinimumLengthErrorMessage);
//        }
        
//        if (digits.HasLengthMoreThan(60, out string? digitsMaximumLengthErrorMessage))
//        {
//            throw new DomainException(digitsMaximumLengthErrorMessage);
//        }

//        string phoneNumber = $"+{countryCode}{digits}";
        
//        if (phoneNumber.IsNotPhoneNumber(out string? phoneNumberErrorMessage))
//        {
//            throw new DomainException(phoneNumberErrorMessage);
//        }

//        return new PhoneNumber(countryCode, digits);
//    }

//    public static implicit operator string(PhoneNumber phoneNumber)
//    {
//        return phoneNumber.ToString();
//    }

//    public override string ToString()
//    {
//        return UnformattedNumber;
//    }

//    protected override IEnumerable<object?> GetEqualityComponents()
//    {
//        yield return CountryCode;
//        yield return Digits;
//    }
//}
