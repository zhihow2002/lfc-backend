using Foundation.Common.Persistence.Models;
using Foundation.Common.Utilities;
using Foundation.Features.ExceptionHandling.Exceptions;
using Foundation.Features.FileStorage.Models;
using Foundation.Features.Validation.Simple;

namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

public class ExcelFile : BaseValueObject
{
    protected ExcelFile()
    {
    }

    private ExcelFile(string fullName, byte[] data)
    {
        FullName = fullName;
        Name = GetFileName(fullName);
        Extension = GetFileExtension(fullName);
        Size = GetFileSize(data);
        Data = data;
    }

    public byte[] Data { get; private set; } = default!;
    public string FullName { get; private set; } = default!;
    public string Extension { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public long Size { get; private set; }

    public static ExcelFile Create(string fileName, byte[] data)
    {
        if (fileName.IsNullOrWhiteSpace(out string? fileNameNullOrWhiteSpaceErrorMessage))
        {
            throw new DomainException(fileNameNullOrWhiteSpaceErrorMessage);
        }

        if (fileName.HasLengthMoreThan(50, out string? nameLengthErrorMessage))
        {
            throw new DomainException(nameLengthErrorMessage);
        }

        string[] validExtensions = { ".xlsx", ".xls", ".csv" };
        string ext = GetFileExtension(fileName);

        if (validExtensions.All(x => x != ext))
        {
            throw new DomainException($"Excel file only supports '{string.Join(", ", validExtensions)}'");
        }

        if (data.IsNullOrEmpty(out string? dataNullOrEmptyErrorMessage))
        {
            throw new DomainException(dataNullOrEmptyErrorMessage);
        }

        return new ExcelFile(fileName, data);
    }
    
    public static ExcelFile Create(FileUploadBase64String uploadBase64String)
    {
        return Create($"{uploadBase64String.Name}{uploadBase64String.Extension}", FileUtility.ConvertBase64StringToByteArray(uploadBase64String));
    }

    public static implicit operator string(ExcelFile excelFile)
    {
        return excelFile.ToString();
    }

    public override string ToString()
    {
        return FullName;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Data;
        yield return Extension;
        yield return FullName;
        yield return Name;
        yield return Size;
    }

    private static string GetFileExtension(string fileName)
    {
        return Path.GetExtension(fileName);
    }

    private static string GetFileName(string fileName)
    {
        return Path.GetFileNameWithoutExtension(fileName);
    }

    private static long GetFileSize(byte[] fileData)
    {
        return fileData.Length;
    }
}
