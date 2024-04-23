//using Foundation.Common.Persistence.Models;
//using Foundation.Common.Utilities;
//using Foundation.Features.ExceptionHandling.Exceptions;
//using Foundation.Features.Validation.Simple;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class HtmlMaxContent : BaseValueObject
//{
//    private readonly string _value = default!;

//    protected HtmlMaxContent()
//    {
//    }

//    private HtmlMaxContent(string value)
//    {
//        _value = value;
//    }

//    public string Value => _value.ToHtmlEncoded();

//    public static HtmlMaxContent Create(string htmlContent)
//    {
//        if (htmlContent.IsNullOrWhiteSpace(out string? htmlContentNullOrWhiteSpaceErrorMessage))
//        {
//            throw new DomainException(htmlContentNullOrWhiteSpaceErrorMessage);
//        }

//        if (htmlContent.HasLengthMoreThan(25000, out string? htmlContentMaximumLengthErrorMessage))
//        {
//            throw new DomainException(htmlContentMaximumLengthErrorMessage);
//        }

//        return new HtmlMaxContent(htmlContent);
//    }


//    public static implicit operator string(HtmlMaxContent htmlContent)
//    {
//        return htmlContent.ToString();
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
