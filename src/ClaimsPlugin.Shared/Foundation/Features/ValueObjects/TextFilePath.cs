using Foundation.Common.Persistence.Models;
using Foundation.Features.ExceptionHandling.Exceptions;
using Foundation.Features.Validation.Simple;

namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

public class TextFilePath : BaseValueObject
{
    protected TextFilePath()
    {
    }

    private TextFilePath(string value)
    {
        Value = value;
    }

    public string Value { get; private set; } = default!;

    public static TextFilePath Create(string filePath)
    {
        if (filePath.IsNullOrWhiteSpace(out string? filePathNullOrWhiteSpaceErrorMessage))
        {
            throw new DomainException(filePathNullOrWhiteSpaceErrorMessage);
        }

        if (filePath.HasLengthMoreThan(200, out string? filePathLengthErrorMessage))
        {
            throw new DomainException(filePathLengthErrorMessage);
        }

        string[] validExtensions = { ".txt" };

        string ext = Path.GetExtension(filePath).ToLower();

        if (validExtensions.All(x => x != ext))
        {
            throw new DomainException($"Text file only supports '{string.Join(", ", validExtensions)}'");
        }

        return new TextFilePath(filePath);
    }

    public static implicit operator string(TextFilePath filePath)
    {
        return filePath.ToString();
    }


    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
