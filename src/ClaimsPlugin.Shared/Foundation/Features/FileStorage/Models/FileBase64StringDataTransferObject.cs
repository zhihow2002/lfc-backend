namespace ClaimsPlugin.Shared.Foundation.Features.FileStorage.Models;

public class FileBase64StringDataTransferObject
{
    public string Name { get; set; } = default!;
    public string Extension { get; set; } = default!;
    public string Data { get; set; } = default!;
}
