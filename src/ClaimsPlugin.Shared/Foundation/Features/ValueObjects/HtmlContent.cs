//using Foundation.Common.Persistence.Models;
//using Foundation.Common.Utilities;
//using Foundation.Features.ExceptionHandling.Exceptions;
//using Foundation.Features.Validation.Simple;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class HtmlContent : BaseValueObject
//{
//    private readonly string _value = default!;

//    protected HtmlContent()
//    {
//    }

//    private HtmlContent(string value)
//    {
//        _value = value;
//    }

//    public string Value => _value.ToHtmlEncoded();

//    public static HtmlContent Create(string htmlContent)
//    {
//        if (htmlContent.IsNullOrWhiteSpace(out string? htmlContentNullOrWhiteSpaceErrorMessage))
//        {
//            throw new DomainException(htmlContentNullOrWhiteSpaceErrorMessage);
//        }

//        return new HtmlContent(htmlContent);
//    }


//    public static implicit operator string(HtmlContent htmlContent)
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
