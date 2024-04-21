namespace ClaimsPlugin.Shared.Foundation.Features.FileProcessing.Interfaces;
public interface IFileService
{
    public byte[] Zip(List<(string fullName, byte[] data)> files, string? password = null);
    public byte[] Zip((string fullName, byte[] data) file, string? password = null);
    public Task<byte[]> PgpEncrypt(string inputFilePath, string publicKeyFilePath);
    public Task<byte[]> PgpEncrypt(byte[] inputFile, byte[] publicKeyFile);
    public Task<byte[]> PgpDecrypt(string inputFilePath, string privateKeyFilePath, string? password);
    public Task<byte[]> PgpDecrypt(byte[] inputFile, byte[] privateKeyFile, string? password);
    public Task<string?> DownloadAsBase64StringAsync(string url);
    public Task<byte[]?> DownloadAsByteArrayAsync(string url);
    public string GetChecksum(string filePath);
    public string GetChecksum(byte[] file);
    public string GetChecksum(MemoryStream stream);
    public bool IsSameFile(string sourceFilePath, string targetFilePath);
    public bool IsSameFile(MemoryStream sourceStream, MemoryStream targetStream);
    public bool IsSameFile(MemoryStream sourceStream, string targetFilePath);
    public bool IsSameFile(string sourceFilePath, MemoryStream targetStream);
}
