using System.Reflection;
using ClaimsPlugin.Shared.Foundation.Features.FileStorage.Models;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
using Microsoft.IO;
namespace ClaimsPlugin.Shared.Foundation.Common.Utilities;
public static class FileUtility
{
    private static readonly RecyclableMemoryStreamManager RecyclableMemoryStreamManager = new();
    public class FormatBuilder
    {
        private readonly List<FileFormat> _fileFormats = new();
        public FormatBuilder Add(FileFormat[] formats)
        {
            _fileFormats.AddRange(formats);
            return this;
        }
        public FormatBuilder Add(FileFormat format)
        {
            _fileFormats.Add(format);
            return this;
        }
        public FileFormat[] Build()
        {
            return _fileFormats.ToArray();
        }
    }
    public static byte[] ConvertFormFileToByteArray(FileUploadForm formFile)
    {
        if (formFile.File.Length <= 0)
        {
            return Array.Empty<byte>();
        }
        using MemoryStream ms = RecyclableMemoryStreamManager.GetStream();
        formFile.File.CopyTo(ms);
        return ms.ToArray();
    }
    public static string ConvertFormFileToBase64String(FileUploadForm formFile)
    {
        if (formFile.File.Length <= 0)
        {
            return string.Empty;
        }
        using MemoryStream ms = RecyclableMemoryStreamManager.GetStream();
        formFile.File.CopyTo(ms);
        return Convert.ToBase64String(ms.ToArray());
    }
    public static MemoryStream GetMemoryStream()
    {
        return RecyclableMemoryStreamManager.GetStream();
    }
    public static FileUploadForm ConvertBase64StringToFormFile(MemoryStream memoryStream, FileUploadBase64String fileUploadBase64String)
    {
        byte[] bytes = Convert.FromBase64String(fileUploadBase64String.Data);
        memoryStream.Write(bytes, 0, bytes.Length);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return new FileUploadForm
        {
            File = new FormFile(memoryStream, 0, bytes.Length, fileUploadBase64String.Name, fileUploadBase64String.Name)
        };
    }
    public static byte[] ConvertBase64StringToByteArray(FileUploadBase64String uploadBase64String)
    {
        return Convert.FromBase64String(uploadBase64String.Data);
    }
    public static string GetFileFormatsInString(params FileFormat[] formats)
    {
        return string.Join(",", formats.Select(x => x.Extension));
    }
    public static long GetFileSize(long size, UnitsOfMeasurement unitsOfMeasurement = UnitsOfMeasurement.MegaByte)
    {
        return size * (int)unitsOfMeasurement;
    }
    public static long GetFileSize(string base64String, bool applyPaddingsRules = true, UnitsOfMeasurement unitsOfMeasurement = UnitsOfMeasurement.MegaByte)
    {
        if (string.IsNullOrEmpty(base64String)) return 0;
        int base64Length = base64String.AsSpan()[(base64String.IndexOf(',') + 1)..].Length;
        double fileSizeInByte = Math.Ceiling((double)base64Length / 4) * 3;
        if (applyPaddingsRules && base64Length >= 2)
        {
            ReadOnlySpan<char> paddings = base64String.AsSpan()[^2..];
            fileSizeInByte = paddings.EndsWith("==")
                ? fileSizeInByte - 2
                : paddings[1].Equals('=') ? fileSizeInByte - 1 : fileSizeInByte;
        }
        return (long)(fileSizeInByte > 0 ? fileSizeInByte / (int)unitsOfMeasurement : 0);
    }
    public static bool IsContentTypeMatched(string contentType, params FileFormat[] formats)
    {
        return formats.Select(x => x.ContentType).Any(x => x.IsEqualTo(contentType, StringComparison.OrdinalIgnoreCase));
    }
    public static bool IsContentTypeMatched(string contentType, params FileFormat[][] formats)
    {
        return formats.SelectMany(x => x).Select(x => x.ContentType).Any(x => x.IsEqualTo(contentType, StringComparison.OrdinalIgnoreCase));
    }
    public static bool IsExtensionMatched(string fileName, params FileFormat[] formats)
    {
        return formats.Select(x => x.Extension).Any(x => fileName.IsEndWith(x, StringComparison.OrdinalIgnoreCase));
    }
    public static bool IsExtensionMatched(string fileName, params FileFormat[][] formats)
    {
        return formats.SelectMany(x => x).Select(x => x.Extension).Any(x => fileName.IsEndWith(x, StringComparison.OrdinalIgnoreCase));
    }
    public static bool IsExtensionSupported(string extension)
    {
        return GetAllFileFormats().Any(x => x.Extension.IsEqualTo(extension, StringComparison.OrdinalIgnoreCase));
    }
    public static FileFormat GetFileFormatFromFileName(string fileName)
    {
        string extension = Path.GetExtension(fileName).ToLower();
        FileFormat? fileFormat = GetAllFileFormats().FirstOrDefault(x => string.Equals(x.Extension, extension, StringComparison.OrdinalIgnoreCase));
        if (fileFormat.IsNull())
        {
            throw new InvalidOperationException("This extension is unsupported.");
        }
        return fileFormat;
    }
    public static FileFormat GetFileFormatFromFileExtension(string extension)
    {
        FileFormat? fileFormat = GetAllFileFormats().FirstOrDefault(x => string.Equals(x.Extension, extension, StringComparison.OrdinalIgnoreCase));
        if (fileFormat.IsNull())
        {
            throw new InvalidOperationException("This extension is unsupported.");
        }
        return fileFormat;
    }
    public static IEnumerable<FileFormat> GetAllFileFormats()
    {
        IEnumerable<MethodInfo> methods = typeof(FileUtility).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(x => x.ReturnType == typeof(FileUtility.FileFormat) && !x.ContainsGenericParameters && x.GetParameters().Length == 0);
        foreach (MethodInfo methodInfo in methods)
        {
            yield return (FileFormat)methodInfo.Invoke(null, null)!;
        }
    }
    public static FileFormat[] GetStandardExcelFileFormats()
    {
        return new[] { GetCsvFileFormat(), GetXlsFileFormat(), GetXlsxFileFormat() };
    }
    public static FileFormat[] GetStandardImageFileFormats()
    {
        return new[] { GetJpegFileFormat(), GetJpgFileFormat(), GetPngFileFormat() };
    }
    public static FileFormat[] GetStandardFontFileFormats()
    {
        return new[] { GetOtfFileFormat(), GetTtfFileFormat(), GetWoffFileFormat(), GetWoff2FileFormat() };
    }
    public static FormatBuilder InitFormatBuilder()
    {
        return new FormatBuilder();
    }
    public static FileFormat GetCsvFileFormat()
    {
        return new FileFormat(".csv", "text/csv");
    }
    public static FileFormat GetXlsxFileFormat()
    {
        return new FileFormat(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }
    public static FileFormat GetXlsFileFormat()
    {
        return new FileFormat(".xls", "application/vnd.ms-excel");
    }
    public static FileFormat GetTxtFileFormat()
    {
        return new FileFormat(".txt", "text/plain");
    }
    public static FileFormat GetPngFileFormat()
    {
        return new FileFormat(".png", "image/png");
    }
    public static FileFormat GetJpgFileFormat()
    {
        return new FileFormat(".jpg", "image/jpeg");
    }
    public static FileFormat GetJpegFileFormat()
    {
        return new FileFormat(".jpeg", "image/jpeg");
    }
    public static FileFormat GetWebPFileFormat()
    {
        return new FileFormat(".webp", "image/webp");
    }
    public static FileFormat GetPdfFileFormat()
    {
        return new FileFormat(".pdf", "application/pdf");
    }
    public static FileFormat GetHtmlFileFormat()
    {
        return new FileFormat(".html", "text/html");
    }
    public static FileFormat GetJsonFileFormat()
    {
        return new FileFormat(".json", "application/json");
    }
    public static FileFormat GetPptxFileFormat()
    {
        return new FileFormat(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
    }
    public static FileFormat GetPptFileFormat()
    {
        return new FileFormat(".ppt", "application/vnd.ms-powerpoint");
    }
    public static FileFormat GetZipFileFormat()
    {
        return new FileFormat(".zip", "application/zip");
    }
    public static FileFormat Get7ZipFileFormat()
    {
        return new FileFormat(".7z", "application/x-7z-compressed");
    }
    public static FileFormat GetDocxFileFormat()
    {
        return new FileFormat(".docx", "application/msword");
    }
    public static FileFormat GetDocFileFormat()
    {
        return new FileFormat(".doc", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
    }
    public static FileFormat GetMsgFileFormat()
    {
        return new FileFormat(".msg", "application/vnd.ms-outlook");
    }
    public static FileFormat GetWoffFileFormat()
    {
        return new FileFormat(".woff", "font/woff");
    }
    public static FileFormat GetWoff2FileFormat()
    {
        return new FileFormat(".woff2", "font/woff2");
    }
    public static FileFormat GetTtfFileFormat()
    {
        return new FileFormat(".ttf", "font/ttf");
    }
    public static FileFormat GetOtfFileFormat()
    {
        return new FileFormat(".otf", "font/otf");
    }
    public class FileFormat : IComparable, IComparable<FileFormat>
    {
        private bool Equals(FileFormat other)
        {
            return Extension == other.Extension && ContentType == other.ContentType;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((FileFormat)obj);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Extension, ContentType);
        }
        internal FileFormat(string extension, string contentType)
        {
            Extension = extension;
            ContentType = contentType;
        }
        public string Extension { get; }
        public string ContentType { get; }
        public static bool operator ==(FileFormat? a, FileFormat? b)
        {
            if (a is null && b is null)
            {
                return true;
            }
            if (a is null || b is null)
            {
                return false;
            }
            return a.Equals(b);
        }
        public static bool operator !=(FileFormat a, FileFormat b)
        {
            return !(a == b);
        }
        public int CompareTo(object? obj)
        {
            return CompareComponents(this, obj);
        }
        public int CompareTo(FileFormat? other)
        {
            return CompareTo(other as object);
        }
        private static int CompareComponents(object? object1, object? object2)
        {
            if (object1 is null && object2 is null)
            {
                return 0;
            }
            if (object1 is null)
            {
                return -1;
            }
            if (object2 is null)
            {
                return 1;
            }
            if (object1 is IComparable comparable1 && object2 is IComparable comparable2)
            {
                return comparable1.CompareTo(comparable2);
            }
            return object1.Equals(object2) ? 0 : -1;
        }
    }
    public enum UnitsOfMeasurement
    {
        Byte = 1,
        KiloByte = 1_024,
        MegaByte = 1_048_576
    }
}
