using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using ClaimsPlugin.Shared.Foundation.Common.Utilities;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;
using ClaimsPlugin.Shared.Foundation.Features.FileStorage.Models;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

namespace ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.ValueObjects;

public class File : BaseValueObject
{
    protected File() { }

    private File(string fullName, byte[] data)
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

    public static File Create(string fileName, byte[] data)
    {
        if (fileName.IsNullOrWhiteSpace(out string? fileNameNullOrWhiteSpaceErrorMessage))
        {
            throw new DomainException(fileNameNullOrWhiteSpaceErrorMessage);
        }
        if (fileName.HasLengthMoreThan(50, out string? nameLengthErrorMessage))
        {
            throw new DomainException(nameLengthErrorMessage);
        }
        string[] validExtensions =
        {
            ".txt",
            ".xlsx",
            ".xls",
            ".csv",
            ".jpg",
            ".jpeg",
            ".gif",
            ".tif",
            ".png",
            ".pdf"
        };
        string ext = GetFileExtension(fileName).ToLower();
        if (validExtensions.All(x => x != ext))
        {
            throw new DomainException($"File only supports '{string.Join(", ", validExtensions)}'");
        }
        if (ext.HasLengthMoreThan(10, out string? extensionLengthErrorMessage))
        {
            throw new DomainException(extensionLengthErrorMessage);
        }
        if (data.IsNullOrEmpty(out string? dataNullOrEmptyErrorMessage))
        {
            throw new DomainException(dataNullOrEmptyErrorMessage);
        }
        return new File(fileName, data);
    }

    public static File Create(FileUploadBase64String uploadBase64String)
    {
        return Create(
            $"{uploadBase64String.Name}{uploadBase64String.Extension}",
            FileUtility.ConvertBase64StringToByteArray(uploadBase64String)
        );
    }

    public static File Create(FileUploadForm formFile)
    {
        return Create(formFile.File.FileName, FileUtility.ConvertFormFileToByteArray(formFile));
    }

    public static implicit operator string(File file)
    {
        return file.ToString();
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
