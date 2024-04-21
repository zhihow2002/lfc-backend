using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;
namespace ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.ValueObjects;
public class ImageFilePath : BaseValueObject
{
    protected ImageFilePath() { }
    private ImageFilePath(string value)
    {
        Value = value;
    }
    public string Value { get; private set; } = default!;
    public static ImageFilePath Create(string filePath)
    {
        if (filePath.IsNullOrWhiteSpace(out string? filePathNullOrWhiteSpaceErrorMessage))
        {
            throw new DomainException(filePathNullOrWhiteSpaceErrorMessage);
        }
        if (filePath.HasLengthMoreThan(200, out string? filePathLengthErrorMessage))
        {
            throw new DomainException(filePathLengthErrorMessage);
        }
        string[] validExtensions = { ".jpg", ".jpeg", ".gif", ".tif", ".png" };
        string ext = Path.GetExtension(filePath).ToLower();
        if (validExtensions.All(x => x != ext))
        {
            throw new DomainException(
                $"Image only supports '{string.Join(", ", validExtensions)}'"
            );
        }
        return new ImageFilePath(filePath);
    }
    public static implicit operator string(ImageFilePath filePath)
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
