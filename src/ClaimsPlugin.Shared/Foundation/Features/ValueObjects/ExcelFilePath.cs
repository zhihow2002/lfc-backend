//using Foundation.Common.Persistence.Models;
//using Foundation.Features.ExceptionHandling.Exceptions;
//using Foundation.Features.Validation.Simple;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class ExcelFilePath : BaseValueObject
//{
//    protected ExcelFilePath()
//    {
//    }

//    private ExcelFilePath(string value)
//    {
//        Value = value;
//    }

//    public string Value { get; private set; } = default!;

//    public static ExcelFilePath Create(string filePath)
//    {
//        if (filePath.IsNullOrWhiteSpace(out string? filePathNullOrWhiteSpaceErrorMessage))
//        {
//            throw new DomainException(filePathNullOrWhiteSpaceErrorMessage);
//        }

//        if (filePath.HasLengthMoreThan(200, out string? filePathLengthErrorMessage))
//        {
//            throw new DomainException(filePathLengthErrorMessage);
//        }

//        string[] validExtensions = { ".xlsx", ".xls", ".csv" };

//        string ext = Path.GetExtension(filePath).ToLower();

//        if (validExtensions.All(x => x != ext))
//        {
//            throw new DomainException($"Excel file only supports '{string.Join(", ", validExtensions)}'");
//        }

//        return new ExcelFilePath(filePath);
//    }


//    public static implicit operator string(ExcelFilePath filePath)
//    {
//        return filePath.ToString();
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
