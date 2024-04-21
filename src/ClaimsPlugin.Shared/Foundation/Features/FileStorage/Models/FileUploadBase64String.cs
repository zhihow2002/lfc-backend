namespace ClaimsPlugin.Shared.Foundation.Features.FileStorage.Models;

public class FileUploadBase64String
{
    public string Name { get; set; } = default!;
    public string Extension { get; set; } = default!;
    public string Data { get; set; } = default!;
}
