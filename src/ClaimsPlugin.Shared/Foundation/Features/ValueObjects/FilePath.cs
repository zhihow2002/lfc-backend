//using Foundation.Common.Persistence.Models;
//using Foundation.Features.ExceptionHandling.Exceptions;
//using Foundation.Features.Validation.Simple;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class FilePath : BaseValueObject
//{
//    protected FilePath()
//    {
//    }

//    private FilePath(string value)
//    {
//        Value = value;
//    }

//    public string Value { get; private set; } = default!;

//    public static FilePath Create(string filePath)
//    {
//        if (filePath.IsNullOrWhiteSpace(out string? filePathNullOrWhiteSpaceErrorMessage))
//        {
//            throw new DomainException(filePathNullOrWhiteSpaceErrorMessage);
//        }

//        if (filePath.HasLengthMoreThan(200, out string? filePathLengthErrorMessage))
//        {
//            throw new DomainException(filePathLengthErrorMessage);
//        }

//        string[] validExtensions = { ".txt", ".xlsx", ".xls", ".csv", ".jpg", ".jpeg", ".gif", ".tif", ".png" };

//        string ext = Path.GetExtension(filePath).ToLower();

//        if (validExtensions.All(x => x != ext))
//        {
//            throw new DomainException($"File only supports '{string.Join(", ", validExtensions)}'");
//        }

//        return new FilePath(filePath);
//    }


//    public static implicit operator string(FilePath filePath)
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
