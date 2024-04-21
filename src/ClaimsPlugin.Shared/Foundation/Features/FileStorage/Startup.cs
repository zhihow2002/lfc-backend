using ClaimsPlugin.Shared.Foundation.Features.FileStorage.Models;
using Microsoft.Extensions.FileProviders;
namespace Foundation.Features.FileStorage;
internal static class Startup
{
    internal static IServiceCollection AddFileStorage(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<FileStorageSettings>(configuration.GetSection(nameof(FileStorageSettings)));
    }
    internal static IApplicationBuilder UseFileStorage(this IApplicationBuilder app)
    {
        string directory = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Uploads");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        return app.UseStaticFiles(
            new StaticFileOptions { FileProvider = new PhysicalFileProvider(directory), RequestPath = new PathString("/Assets/Uploads") }
        );
    }
}
