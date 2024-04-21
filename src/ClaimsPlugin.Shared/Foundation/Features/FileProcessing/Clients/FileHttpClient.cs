using ClaimsPlugin.Shared.Foundation.Features.HttpClient.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
using ClaimsPlugin.Shared.Foundation.HttpClient.Models;
using RestSharp;
namespace ClaimsPlugin.Shared.Foundation.Features.FileProcessing.Clients;
internal class FileHttpClient : BaseHttpClient, IHttpClient
{
    public async Task<string?> DownloadAsBase64StringAsync(string url)
    {
        byte[]? bytes = await DownloadAsByteArrayAsync(url);
        return bytes.IsNull() ? null : Convert.ToBase64String(bytes);
    }
    public async Task<byte[]?> DownloadAsByteArrayAsync(string url)
    {
        return await Client.DownloadDataAsync(new RestRequest(url));
    }
    public string GetClientName()
    {
        return "File Processing";
    }
}
