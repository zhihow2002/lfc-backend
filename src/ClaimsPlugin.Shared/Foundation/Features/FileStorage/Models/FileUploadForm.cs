using Microsoft.AspNetCore.Http;

namespace ClaimsPlugin.Shared.Foundation.Features.FileStorage.Models;

public class FileUploadForm
{
    public IFormFile File { get; set; } = default!;
}
