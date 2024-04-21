using System.Security.Cryptography;
using ClaimsPlugin.Shared.Foundation.Features.FileProcessing.Clients;
using ClaimsPlugin.Shared.Foundation.Features.FileProcessing.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.FileStorage.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
using Ionic.Zip;
using Microsoft.IO;
using PgpCore;
namespace ClaimsPlugin.Shared.Foundation.Features.FileProcessing.Services;
public class FileService : IFileService
{
    private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager = new();
    private readonly FileHttpClient _fileHttpClient = new();
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<FileService> _logger;
    public FileService(IFileStorageService fileStorageService, ILogger<FileService> logger)
    {
        _fileStorageService = fileStorageService;
        _logger = logger;
    }
    public byte[] Zip(List<(string fullName, byte[] data)> files, string? password = null)
    {
        using ZipFile zip = new();
        if (password.IsNotNullOrWhiteSpace())
        {
            zip.Password = password;
            zip.Encryption = EncryptionAlgorithm.WinZipAes256;
        }
        foreach ((string fullName, byte[] data) file in files)
        {
            zip.AddEntry(file.fullName, file.data);
        }
        using MemoryStream? output = _recyclableMemoryStreamManager.GetStream();
        zip.Save(output);
        return output.ToArray();
    }
    public byte[] Zip((string fullName, byte[] data) file, string? password = null)
    {
        return Zip(new List<(string fullName, byte[] data)> { file }, password);
    }
    public async Task<byte[]> PgpEncrypt(string inputFilePath, string publicKeyFilePath)
    {
        if (!File.Exists(inputFilePath))
        {
            throw new ArgumentException($"Unable to find the input file in the given location, path: \'{inputFilePath}\'");
        }
        if (!File.Exists(publicKeyFilePath))
        {
            throw new ArgumentException($"Unable to find the public key file in the given location, path: \'{publicKeyFilePath}\'");
        }
        await using FileStream input = new(inputFilePath, FileMode.Open);
        using MemoryStream? output = _recyclableMemoryStreamManager.GetStream();
        using PGP pgp = new(new EncryptionKeys(new FileInfo(publicKeyFilePath)));
        await pgp.EncryptStreamAsync(input, output);
        return output.ToArray();
    }
    public async Task<byte[]> PgpEncrypt(byte[] inputFile, byte[] publicKeyFile)
    {
        if (inputFile.Length <= 0)
        {
            throw new ArgumentException($"Input file is empty");
        }
        if (publicKeyFile.Length <= 0)
        {
            throw new ArgumentException($"Public key file is empty");
        }
        using MemoryStream? key = _recyclableMemoryStreamManager.GetStream();
        using MemoryStream? input = _recyclableMemoryStreamManager.GetStream();
        using MemoryStream? output = _recyclableMemoryStreamManager.GetStream();
        await input.WriteAsync(inputFile);
        await key.WriteAsync(publicKeyFile);
        input.Position = 0;
        key.Position = 0;
        using PGP pgp = new(new EncryptionKeys(key));
        await pgp.EncryptStreamAsync(input, output);
        return output.ToArray();
    }
    public async Task<byte[]> PgpDecrypt(string inputFilePath, string privateKeyFilePath, string? password)
    {
        if (!File.Exists(inputFilePath))
        {
            throw new ArgumentException($"Unable to find the input file in the given location, path: \'{inputFilePath}\'");
        }
        if (!File.Exists(privateKeyFilePath))
        {
            throw new ArgumentException($"Unable to find the private key file in the given location, path: \'{privateKeyFilePath}\'");
        }
        await using FileStream input = new(inputFilePath, FileMode.Open);
        using MemoryStream? output = _recyclableMemoryStreamManager.GetStream();
        using PGP pgp = new(new EncryptionKeys(new FileInfo(privateKeyFilePath), password ?? string.Empty));
        await pgp.DecryptStreamAsync(input, output);
        return output.ToArray();
    }
    public async Task<byte[]> PgpDecrypt(byte[] inputFile, byte[] privateKeyFile, string? password)
    {
        if (inputFile.Length <= 0)
        {
            throw new ArgumentException($"Input file is empty");
        }
        if (privateKeyFile.Length <= 0)
        {
            throw new ArgumentException($"Private key file is empty");
        }
        using MemoryStream? key = _recyclableMemoryStreamManager.GetStream();
        using MemoryStream? input = _recyclableMemoryStreamManager.GetStream();
        using MemoryStream? output = _recyclableMemoryStreamManager.GetStream();
        await input.WriteAsync(inputFile);
        await key.WriteAsync(privateKeyFile);
        input.Position = 0;
        key.Position = 0;
        using PGP pgp = new(new EncryptionKeys(key, password ?? string.Empty));
        await pgp.DecryptStreamAsync(input, output);
        return output.ToArray();
    }
    public async Task<string?> DownloadAsBase64StringAsync(string url)
    {
        return await _fileHttpClient.DownloadAsBase64StringAsync(url);
    }
    public async Task<byte[]?> DownloadAsByteArrayAsync(string url)
    {
        return await _fileHttpClient.DownloadAsByteArrayAsync(url);
    }
    public string GetChecksum(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new ArgumentException("Unable to find the file in the given location.");
        }
        using FileStream stream = File.OpenRead(filePath);
        return ComputeChecksum(stream);
    }
    public string GetChecksum(byte[] file)
    {
        using MemoryStream stream = new(file);
        return ComputeChecksum(stream);
    }
    public string GetChecksum(MemoryStream stream)
    {
        if (stream.Position != 0)
            stream.Position = 0;
        return ComputeChecksum(stream);
    }
    public bool IsSameFile(string sourceFilePath, string targetFilePath)
    {
        return GetChecksum(sourceFilePath) == GetChecksum(targetFilePath);
    }
    public bool IsSameFile(MemoryStream sourceStream, MemoryStream targetStream)
    {
        return GetChecksum(sourceStream) == GetChecksum(targetStream);
    }
    public bool IsSameFile(MemoryStream sourceStream, string targetFilePath)
    {
        return GetChecksum(sourceStream) == GetChecksum(targetFilePath);
    }
    public bool IsSameFile(string sourceFilePath, MemoryStream targetStream)
    {
        return GetChecksum(sourceFilePath) == GetChecksum(targetStream);
    }
    private static string ComputeChecksum(Stream inputStream)
    {
        using MD5 md5Hash = MD5.Create();
        byte[] hash = md5Hash.ComputeHash(inputStream);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
}
