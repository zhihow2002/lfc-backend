using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using ClaimsPlugin.Shared.Foundation.Common.Extensions;
using ClaimsPlugin.Shared.Foundation.Common.Utilities;
using ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.ValueObjects;
using ClaimsPlugin.Shared.Foundation.Features.FileStorage.Helpers;
using ClaimsPlugin.Shared.Foundation.Features.FileStorage.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.FileStorage.Models;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
using Microsoft.Extensions.Options;
using Microsoft.IO;
using File = System.IO.File;

namespace ClaimsPlugin.Shared.Foundation.Features.FileStorage.Services;

public class FileStorageService : IFileStorageService
{
    private const string NumberPattern = "-{0}";
    private readonly string _basePath;
    private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
    private readonly ILogger<FileStorageService> _logger;

    public FileStorageService(
        IOptions<FileStorageSettings> fileStorageSettings,
        ILogger<FileStorageService> logger
    )
    {
        _logger = logger;
        _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();

        _basePath = Path.Join(
            fileStorageSettings.Value.AzureFiles.Enabled
                ? fileStorageSettings.Value.AzureFiles.Path
                : Directory.GetCurrentDirectory(),
            "Assets",
            "Uploads"
        );
    }

    /// <summary>
    ///     Upload file from base 64 string to the specified folder under the asset uploads directory.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="supportedFileFormat"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<string> PushAsync<T>(
        FileUploadBase64String request,
        FileUtility.FileFormat supportedFileFormat,
        CancellationToken cancellationToken = default
    )
        where T : class
    {
        return await PushAsync<T>(request, new[] { supportedFileFormat }, cancellationToken);
    }

    /// <summary>
    ///     Upload file from base 64 string to the specified folder under the asset uploads directory.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="supportedFileFormat"></param>
    /// <param name="folderName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> PushAsync(
        FileUploadBase64String request,
        FileUtility.FileFormat supportedFileFormat,
        string folderName,
        CancellationToken cancellationToken = default
    )
    {
        return await PushAsync(
            request,
            new[] { supportedFileFormat },
            folderName,
            cancellationToken
        );
    }

    /// <summary>
    ///     Upload file from base 64 string to the specified folder under the asset uploads directory.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="supportedFileFormats"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<string> PushAsync<T>(
        FileUploadBase64String request,
        FileUtility.FileFormat[] supportedFileFormats,
        CancellationToken cancellationToken = default
    )
        where T : class
    {
        return await UploadAsync(typeof(T), request, supportedFileFormats, cancellationToken);
    }

    public async Task<string> PushAsync<T>(
        byte[] rawData,
        string fileName,
        FileUtility.FileFormat fileFormat,
        CancellationToken cancellationToken = default
    )
        where T : class
    {
        FileUploadBase64String file = new FileUploadBase64String
        {
            Name = fileName,
            Extension = fileFormat.Extension
        };

        return await UploadAsync(typeof(T), file, new[] { fileFormat }, cancellationToken, rawData);
    }

    /// <summary>
    ///     Upload file from base 64 string to the specified folder under the asset uploads directory.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="supportedFileFormats"></param>
    /// <param name="folderName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<string> PushAsync(
        FileUploadBase64String request,
        FileUtility.FileFormat[] supportedFileFormats,
        string folderName,
        CancellationToken cancellationToken = default
    )
    {
        if (request.Data.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException("File must not empty.");
        }

        if (!FileUtility.IsExtensionMatched(request.Extension, supportedFileFormats))
        {
            throw new InvalidOperationException("File format is not supported.");
        }

        if (request.Name.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException("Name is required.");
        }

        if (folderName.IsNullOrWhiteSpace() && folderName.Length.IsGreaterThan(30))
        {
            throw new InvalidOperationException("Folder name is too long. Maximum up to 30.");
        }

        using MemoryStream streamData = _recyclableMemoryStreamManager.GetStream(
            Convert.FromBase64String(request.Data)
        );

        if (streamData.Length > 0)
        {
            folderName = GetFolderName(supportedFileFormats, folderName);
            string pathToSave = Path.Join(_basePath, folderName);

            Directory.CreateDirectory(pathToSave);

            string fileName = request.Name.Trim('"');
            fileName = RemoveSpecialCharacters(fileName);
            fileName = fileName.ReplaceWhitespace("-");
            fileName += request.Extension.Trim();
            string fullPath = Path.Join(pathToSave, fileName);
            string dbPath = Path.Join(folderName, fileName);
            if (File.Exists(dbPath))
            {
                dbPath = NextAvailableFilename(dbPath);
                fullPath = NextAvailableFilename(fullPath);
            }

            await using FileStream stream = new(fullPath, FileMode.Create);
            await streamData.CopyToAsync(stream, cancellationToken);
            dbPath = dbPath.Replace("\\", "/");

            _logger.LogInformation(
                "[{ServiceName}] File uploaded successfully, path: \'{FullPath}\'",
                nameof(FileStorageService),
                fullPath
            );

            return "/" + dbPath;
        }

        _logger.LogError(
            "[{ServiceName}] File uploaded failed due to no stream data",
            nameof(FileStorageService)
        );

        return string.Empty;
    }

    /// <summary>
    ///     Upload file from base 64 string to the specified folder under the asset uploads directory.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="supportedFileFormat"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<string> PushAsync<T>(
        FileUploadForm request,
        FileUtility.FileFormat supportedFileFormat,
        CancellationToken cancellationToken = default
    )
        where T : class
    {
        return await PushAsync<T>(request, new[] { supportedFileFormat }, cancellationToken);
    }

    /// <summary>
    ///     Upload file from <see cref="FileUploadForm"/> to the specified folder under the asset uploads directory.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="supportedFileFormat"></param>
    /// <param name="folderName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> PushAsync(
        FileUploadForm request,
        FileUtility.FileFormat supportedFileFormat,
        string folderName,
        CancellationToken cancellationToken = default
    )
    {
        return await PushAsync(
            request,
            new[] { supportedFileFormat },
            folderName,
            cancellationToken
        );
    }

    /// <summary>
    ///     Upload file from <see cref="FileUploadForm"/> to the specified folder under the asset uploads directory.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="supportedFileFormats"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<string> PushAsync<T>(
        FileUploadForm request,
        FileUtility.FileFormat[] supportedFileFormats,
        CancellationToken cancellationToken = default
    )
        where T : class
    {
        return await UploadAsync(typeof(T), request, supportedFileFormats, cancellationToken);
    }

    /// <summary>
    ///     Upload file from <see cref="FileUploadForm"/> to the specified folder under the asset uploads directory.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="supportedFileFormats"></param>
    /// <param name="folderName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<string> PushAsync(
        FileUploadForm request,
        FileUtility.FileFormat[] supportedFileFormats,
        string folderName,
        CancellationToken cancellationToken = default
    )
    {
        if (request.File.Length.IsZero() || request.File.Length.IsNegative())
        {
            throw new InvalidOperationException("File must not empty.");
        }

        string ext = Path.GetExtension(request.File.FileName);

        if (!FileUtility.IsExtensionMatched(ext, supportedFileFormats))
        {
            throw new InvalidOperationException("File format is not supported.");
        }

        if (request.File.FileName.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException("Name is required.");
        }

        if (folderName.IsNullOrWhiteSpace() && folderName.Length.IsGreaterThan(30))
        {
            throw new InvalidOperationException("Folder name is too long. Maximum up to 30.");
        }

        using MemoryStream streamData = _recyclableMemoryStreamManager.GetStream();
        await using Stream fileStream = request.File.OpenReadStream();
        await fileStream.CopyToAsync(streamData, cancellationToken);

        if (streamData.Length > 0)
        {
            folderName = GetFolderName(supportedFileFormats, folderName);
            string pathToSave = Path.Join(_basePath, folderName);

            Directory.CreateDirectory(pathToSave);

            string fileName = request.File.FileName.Trim('"');
            fileName = RemoveSpecialCharacters(fileName);
            fileName = fileName.ReplaceWhitespace("-");
            fileName += ext.Trim();
            string fullPath = Path.Join(pathToSave, fileName);
            string dbPath = Path.Join(folderName, fileName);
            if (File.Exists(dbPath))
            {
                dbPath = NextAvailableFilename(dbPath);
                fullPath = NextAvailableFilename(fullPath);
            }

            await using FileStream stream = new(fullPath, FileMode.Create);
            await streamData.CopyToAsync(stream, cancellationToken);
            dbPath = dbPath.Replace("\\", "/");

            _logger.LogInformation(
                "[{ServiceName}] File uploaded successfully, path: \'{FullPath}\'",
                nameof(FileStorageService),
                fullPath
            );

            return "/" + dbPath;
        }

        _logger.LogError(
            "[{ServiceName}] File uploaded failed due to no stream data",
            nameof(FileStorageService)
        );

        return string.Empty;
    }

    /// <summary>
    ///     Retrieve file based on the file and folder name in the asset uploads directory
    /// </summary>
    /// <param name="fileName">Name of the file without extension. Case sensitive.</param>
    /// <param name="fileFormat"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T">Folder name will be based on <see cref="T"/></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<byte[]> PullAsByteArrayAsync<T>(
        string fileName,
        FileUtility.FileFormat fileFormat,
        CancellationToken cancellationToken = default
    )
        where T : class
    {
        await Task.CompletedTask;

        if (fileName.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException("File name must be provided.");
        }

        string folderName = GetFolderName(typeof(T), new[] { fileFormat });
        string pathToRead = Path.Join(_basePath, folderName);

        string fullPath = Path.Join(pathToRead, $"{fileName}{fileFormat.Extension}");

        if (File.Exists(fullPath))
        {
            _logger.LogInformation(
                "[{ServiceName}] File retrieved successfully, path: \'{FullPath}\'",
                nameof(FileStorageService),
                fullPath
            );

            return ConvertFileToByteArray(fullPath);
        }

        _logger.LogError(
            "[{ServiceName}] File retrieved failed due to file does not exist, path: \'{FullPath}\'",
            nameof(FileStorageService),
            fullPath
        );

        return Array.Empty<byte>();
    }

    /// <summary>
    ///     Retrieve file based on the file and folder name in the asset uploads directory
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="fileFormat"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<string> PullAsBase64StringAsync<T>(
        string fileName,
        FileUtility.FileFormat fileFormat,
        CancellationToken cancellationToken = default
    )
        where T : class
    {
        return Convert.ToBase64String(
            await PullAsByteArrayAsync<T>(fileName, fileFormat, cancellationToken)
        );
    }

    /// <summary>
    ///     Retrieve file based on the file and folder name in the asset uploads directory
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<byte[]> PullAsByteArrayAsync(
        string filePath,
        CancellationToken cancellationToken = default
    )
    {
        await Task.CompletedTask;

        if (filePath.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException("File path must be provided.");
        }

        string fullPath = Path.Join(_basePath, filePath);

        if (File.Exists(fullPath))
        {
            _logger.LogInformation(
                "[{ServiceName}] File retrieved successfully, path: \'{FullPath}\'",
                nameof(FileStorageService),
                fullPath
            );

            return ConvertFileToByteArray(fullPath);
        }

        return Array.Empty<byte>();
    }

    /// <summary>
    ///     Retrieve file based on the file and folder name in the asset uploads directory
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> PullAsBase64StringAsync(
        string filePath,
        CancellationToken cancellationToken = default
    )
    {
        return Convert.ToBase64String(await PullAsByteArrayAsync(filePath, cancellationToken));
    }

    public async Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(
        string filePath,
        CancellationToken cancellationToken = default
    )
    {
        if (filePath.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException("File path must be provided.");
        }

        return new FileBase64StringDataTransferObject
        {
            Name = Path.GetFileName(filePath),
            Extension = Path.GetExtension(filePath),
            Data = await PullAsBase64StringAsync(filePath, cancellationToken)
        };
    }

    public async Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(
        ImageFile filePath,
        CancellationToken cancellationToken = default
    )
    {
        await Task.CompletedTask;

        return new FileBase64StringDataTransferObject
        {
            Name = filePath.Name,
            Extension = filePath.Extension,
            Data = Convert.ToBase64String(filePath.Data)
        };
    }

    public async Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(
        ImageFilePath filePath,
        CancellationToken cancellationToken = default
    )
    {
        return await PullAsFileBase64StringDataTransferObjectAsync(
            filePath.Value,
            cancellationToken
        );
    }

    public async Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(
        TextFile filePath,
        CancellationToken cancellationToken = default
    )
    {
        await Task.CompletedTask;

        return new FileBase64StringDataTransferObject
        {
            Name = filePath.Name,
            Extension = filePath.Extension,
            Data = Convert.ToBase64String(filePath.Data)
        };
    }

    public async Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(
        TextFilePath filePath,
        CancellationToken cancellationToken = default
    )
    {
        return await PullAsFileBase64StringDataTransferObjectAsync(
            filePath.Value,
            cancellationToken
        );
    }

    public async Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(
        ExcelFile filePath,
        CancellationToken cancellationToken = default
    )
    {
        await Task.CompletedTask;

        return new FileBase64StringDataTransferObject
        {
            Name = filePath.Name,
            Extension = filePath.Extension,
            Data = Convert.ToBase64String(filePath.Data)
        };
    }

    public async Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(
        ExcelFilePath filePath,
        CancellationToken cancellationToken = default
    )
    {
        return await PullAsFileBase64StringDataTransferObjectAsync(
            filePath.Value,
            cancellationToken
        );
    }

    public async Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(
        DomainDrivenDesign.ValueObjects.File filePath,
        CancellationToken cancellationToken = default
    )
    {
        await Task.CompletedTask;

        return new FileBase64StringDataTransferObject
        {
            Name = filePath.Name,
            Extension = filePath.Extension,
            Data = Convert.ToBase64String(filePath.Data)
        };
    }

    public async Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(
        FilePath filePath,
        CancellationToken cancellationToken = default
    )
    {
        return await PullAsFileBase64StringDataTransferObjectAsync(
            filePath.Value,
            cancellationToken
        );
    }

    /// <summary>
    ///     Retrieve file based on the file and folder name in the asset uploads directory
    /// </summary>
    /// <param name="fileName">Name of the file without extension. Case sensitive.</param>
    /// <param name="fileFormat">Format of the file.</param>
    /// <param name="folderName">Name of the folder. Case sensitive.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<byte[]> PullAsByteArrayAsync(
        string fileName,
        FileUtility.FileFormat fileFormat,
        string folderName,
        CancellationToken cancellationToken = default
    )
    {
        await Task.CompletedTask;

        if (fileName.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException("File name must be provided.");
        }

        folderName = GetFolderName(new[] { fileFormat }, folderName);
        string pathToRead = Path.Join(_basePath, folderName);

        string fullPath = Path.Join(pathToRead, $"{fileName}{fileFormat.Extension}");

        if (File.Exists(fullPath))
        {
            _logger.LogInformation(
                "[{ServiceName}] File retrieved successfully, path: \'{FullPath}\'",
                nameof(FileStorageService),
                fullPath
            );

            return ConvertFileToByteArray(fullPath);
        }

        _logger.LogError(
            "[{ServiceName}] File retrieved failed due to file does not exist, path: \'{FullPath}\'",
            nameof(FileStorageService),
            fullPath
        );

        return Array.Empty<byte>();
    }

    /// <summary>
    ///     Retrieve file based on the file and folder name in the asset uploads directory
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="fileFormat"></param>
    /// <param name="folderName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> PullAsBase64StringAsync(
        string fileName,
        FileUtility.FileFormat fileFormat,
        string folderName,
        CancellationToken cancellationToken = default
    )
    {
        return Convert.ToBase64String(
            await PullAsByteArrayAsync(fileName, fileFormat, folderName, cancellationToken)
        );
    }

    /// <summary>
    ///     Retrieve file path based on the file and folder name in the asset uploads directory
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="fileFormat"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T">Folder name will be based on <see cref="T"/></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<string> GetFilePathAsync<T>(
        string fileName,
        FileUtility.FileFormat fileFormat,
        CancellationToken cancellationToken = default
    )
        where T : class
    {
        await Task.CompletedTask;

        if (fileName.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException("File name must be provided.");
        }

        string folderName = GetFolderName(typeof(T), new[] { fileFormat });
        string pathToRead = Path.Join(_basePath, folderName);

        string fullPath = Path.Join(pathToRead, $"{fileName}{fileFormat.Extension}");

        if (File.Exists(fullPath))
        {
            _logger.LogInformation(
                "[{ServiceName}] File path retrieved successfully, path: \'{FullPath}\'",
                nameof(FileStorageService),
                fullPath
            );

            return fullPath;
        }

        _logger.LogError(
            "[{ServiceName}] File path retrieved failed due to file does not exist, path: \'{FullPath}\'",
            nameof(FileStorageService),
            fullPath
        );

        return string.Empty;
    }

    /// <summary>
    ///     Retrieve file path based on the file and folder name in the asset uploads directory
    /// </summary>
    /// <param name="fileName">Name of the file without extension. Case sensitive.</param>
    /// <param name="fileFormat">Format of the file.</param>
    /// <param name="folderName">Name of the folder. Case sensitive.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<string> GetFilePathAsync(
        string fileName,
        FileUtility.FileFormat fileFormat,
        string folderName,
        CancellationToken cancellationToken = default
    )
    {
        await Task.CompletedTask;

        if (fileName.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException("File name must be provided.");
        }

        folderName = GetFolderName(new[] { fileFormat }, folderName);
        string pathToRead = Path.Join(_basePath, folderName);

        string fullPath = Path.Join(pathToRead, $"{fileName}{fileFormat.Extension}");

        if (File.Exists(fullPath))
        {
            _logger.LogInformation(
                "[{ServiceName}] File path retrieved successfully, path: \'{FullPath}\'",
                nameof(FileStorageService),
                fullPath
            );

            return fullPath;
        }

        _logger.LogError(
            "[{ServiceName}] File path retrieved failed due to file does not exist, path: \'{FullPath}\'",
            nameof(FileStorageService),
            fullPath
        );

        return string.Empty;
    }

    /// <summary>
    ///     Remove file based on the file name in the asset uploads directory
    /// </summary>
    /// <param name="fileName">Name of the file without extension. Case sensitive.</param>
    /// <param name="fileFormat"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    public async Task DeleteAsync<T>(
        string fileName,
        FileUtility.FileFormat fileFormat,
        CancellationToken cancellationToken = default
    )
        where T : class
    {
        await RemoveAsync(typeof(T), fileName, fileFormat, cancellationToken);
    }

    /// <summary>
    ///     Remove file based on the file name in the asset uploads directory
    /// </summary>
    /// <param name="fileName">Name of the file without extension. Case sensitive.</param>
    /// <param name="fileFormat">Format of the file.</param>
    /// <param name="folderName">Name of the folder. Case sensitive.</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task DeleteAsync(
        string fileName,
        FileUtility.FileFormat fileFormat,
        string folderName,
        CancellationToken cancellationToken = default
    )
    {
        await Task.CompletedTask;

        if (fileName.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException("File name must be provided.");
        }

        folderName = GetFolderName(new[] { fileFormat }, folderName);

        string pathToDelete = Path.Join(_basePath, folderName);

        string fullPath = Path.Join(pathToDelete, $"{fileName}{fileFormat.Extension}");

        if (File.Exists(fullPath))
        {
            _logger.LogInformation(
                "[{ServiceName}] File removed, path: \'{FullPath}\'",
                nameof(FileStorageService),
                fullPath
            );

            File.Delete(fullPath);

            return;
        }

        _logger.LogWarning(
            "[{ServiceName}] File removed skipped due to file does not exist, path: \'{FullPath}\'",
            nameof(FileStorageService),
            fullPath
        );
    }

    /// <summary>
    ///     Remove file based on the given path
    /// </summary>
    /// <param name="filePath">Case sensitive</param>
    /// <param name="cancellationToken"></param>
    public async Task DeleteAsync(string filePath, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;

        if (filePath.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException("File path must be provided.");
        }

        string fullPath = Path.Join(_basePath, filePath);

        if (File.Exists(fullPath))
        {
            _logger.LogInformation(
                "[{ServiceName}] File removed, path: \'{FullPath}\'",
                nameof(FileStorageService),
                fullPath
            );

            File.Delete(fullPath);

            return;
        }

        _logger.LogWarning(
            "[{ServiceName}] File removed skipped due to file does not exist, path: \'{FullPath}\'",
            nameof(FileStorageService),
            fullPath
        );
    }

    /// <summary>
    ///     Preview file upload path without saving the file.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="supportedFileFormat"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<string> GetPushPathAsync<T>(
        FileUploadBase64String request,
        FileUtility.FileFormat supportedFileFormat,
        CancellationToken cancellationToken = default
    )
        where T : class
    {
        return await GetPushPathAsync<T>(request, new[] { supportedFileFormat }, cancellationToken);
    }

    /// <summary>
    ///     Preview file upload path without saving the file.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="supportedFileFormat"></param>
    /// <param name="folderName">Name of the folder. Case sensitive.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> GetPushPathAsync(
        FileUploadBase64String request,
        FileUtility.FileFormat supportedFileFormat,
        string folderName,
        CancellationToken cancellationToken = default
    )
    {
        return await GetPushPathAsync(
            request,
            new[] { supportedFileFormat },
            folderName,
            cancellationToken
        );
    }

    /// <summary>
    ///     Preview file upload path without saving the file.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="supportedFileFormats"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<string> GetPushPathAsync<T>(
        FileUploadBase64String request,
        FileUtility.FileFormat[] supportedFileFormats,
        CancellationToken cancellationToken = default
    )
        where T : class
    {
        await Task.CompletedTask;

        if (request.Data.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException("File must not empty.");
        }

        if (!FileUtility.IsExtensionMatched(request.Extension, supportedFileFormats))
        {
            throw new InvalidOperationException("File format is not supported.");
        }

        if (request.Name.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException("Name is required.");
        }

        string folderName = GetFolderName(typeof(T), supportedFileFormats);
        string pathToSave = Path.Join(_basePath, folderName);

        Directory.CreateDirectory(pathToSave);

        string fileName = request.Name.Trim('"');
        fileName = RemoveSpecialCharacters(fileName);
        fileName = fileName.ReplaceWhitespace("-");
        fileName += request.Extension.Trim();
        string fullPath = Path.Join(pathToSave, fileName);
        string dbPath = Path.Join(folderName, fileName);
        if (File.Exists(dbPath))
        {
            dbPath = NextAvailableFilename(dbPath);
            fullPath = NextAvailableFilename(fullPath);
        }

        dbPath = dbPath.Replace("\\", "/");

        _logger.LogInformation(
            "[{ServiceName}] File upload path retrieved successfully, path: \'{FullPath}\'",
            nameof(FileStorageService),
            fullPath
        );

        return "/" + dbPath;
    }

    /// <summary>
    ///     Preview file upload path without saving the file.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="supportedFileFormats"></param>
    /// <param name="folderName">Name of the folder. Case sensitive.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<string> GetPushPathAsync(
        FileUploadBase64String request,
        FileUtility.FileFormat[] supportedFileFormats,
        string folderName,
        CancellationToken cancellationToken = default
    )
    {
        await Task.CompletedTask;

        if (request.Data.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException("File must not empty.");
        }

        if (!FileUtility.IsExtensionMatched(request.Extension, supportedFileFormats))
        {
            throw new InvalidOperationException("File format is not supported.");
        }

        if (request.Name.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException("Name is required.");
        }

        if (folderName.IsNullOrWhiteSpace() && folderName.Length.IsGreaterThan(30))
        {
            throw new InvalidOperationException("Folder name is too long. Maximum up to 30.");
        }

        folderName = GetFolderName(supportedFileFormats, folderName);
        string pathToSave = Path.Join(_basePath, folderName);

        Directory.CreateDirectory(pathToSave);

        string fileName = request.Name.Trim('"');
        fileName = RemoveSpecialCharacters(fileName);
        fileName = fileName.ReplaceWhitespace("-");
        fileName += request.Extension.Trim();
        string fullPath = Path.Join(pathToSave, fileName);
        string dbPath = Path.Join(folderName, fileName);
        if (File.Exists(dbPath))
        {
            dbPath = NextAvailableFilename(dbPath);
            fullPath = NextAvailableFilename(fullPath);
        }

        dbPath = dbPath.Replace("\\", "/");

        _logger.LogInformation(
            "[{ServiceName}] File path retrieved, path: \'{FullPath}\'",
            nameof(FileStorageService),
            fullPath
        );

        return "/" + dbPath;
    }

    /// <summary>
    ///     Preview file upload path without saving the file.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="supportedFileFormat"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<string> GetPushPathAsync<T>(
        FileUploadForm request,
        FileUtility.FileFormat supportedFileFormat,
        CancellationToken cancellationToken = default
    )
        where T : class
    {
        return await GetPushPathAsync<T>(request, new[] { supportedFileFormat }, cancellationToken);
    }

    /// <summary>
    ///     Preview file upload path without saving the file.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="supportedFileFormat"></param>
    /// <param name="folderName">Name of the folder. Case sensitive.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> GetPushPathAsync(
        FileUploadForm request,
        FileUtility.FileFormat supportedFileFormat,
        string folderName,
        CancellationToken cancellationToken = default
    )
    {
        return await GetPushPathAsync(
            request,
            new[] { supportedFileFormat },
            folderName,
            cancellationToken
        );
    }

    /// <summary>
    ///     Preview file upload path without saving the file.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="supportedFileFormats"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<string> GetPushPathAsync<T>(
        FileUploadForm request,
        FileUtility.FileFormat[] supportedFileFormats,
        CancellationToken cancellationToken = default
    )
        where T : class
    {
        await Task.CompletedTask;

        if (request.File.Length.IsZero() || request.File.Length.IsNegative())
        {
            throw new InvalidOperationException("File must not empty.");
        }

        string ext = Path.GetExtension(request.File.FileName);

        if (!FileUtility.IsExtensionMatched(ext, supportedFileFormats))
        {
            throw new InvalidOperationException("File format is not supported.");
        }

        if (request.File.FileName.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException("Name is required.");
        }

        string folderName = GetFolderName(typeof(T), supportedFileFormats);
        string pathToSave = Path.Join(_basePath, folderName);

        Directory.CreateDirectory(pathToSave);

        string fileName = request.File.FileName.Trim('"');
        fileName = RemoveSpecialCharacters(fileName);
        fileName = fileName.ReplaceWhitespace("-");
        fileName += ext.Trim();
        string fullPath = Path.Join(pathToSave, fileName);
        string dbPath = Path.Join(folderName, fileName);
        if (File.Exists(dbPath))
        {
            dbPath = NextAvailableFilename(dbPath);
            fullPath = NextAvailableFilename(fullPath);
        }

        dbPath = dbPath.Replace("\\", "/");

        _logger.LogInformation(
            "[{ServiceName}] File upload path retrieved successfully, path: \'{FullPath}\'",
            nameof(FileStorageService),
            fullPath
        );

        return "/" + dbPath;
    }

    /// <summary>
    ///     Preview file upload path without saving the file.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="supportedFileFormats"></param>
    /// <param name="folderName">Name of the folder. Case sensitive.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<string> GetPushPathAsync(
        FileUploadForm request,
        FileUtility.FileFormat[] supportedFileFormats,
        string folderName,
        CancellationToken cancellationToken = default
    )
    {
        await Task.CompletedTask;

        if (request.File.Length.IsZero() || request.File.Length.IsNegative())
        {
            throw new InvalidOperationException("File must not empty.");
        }

        string ext = Path.GetExtension(request.File.FileName);

        if (!FileUtility.IsExtensionMatched(ext, supportedFileFormats))
        {
            throw new InvalidOperationException("File format is not supported.");
        }

        if (request.File.FileName.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException("Name is required.");
        }

        if (folderName.IsNullOrWhiteSpace() && folderName.Length.IsGreaterThan(30))
        {
            throw new InvalidOperationException("Folder name is too long. Maximum up to 30.");
        }

        folderName = GetFolderName(supportedFileFormats, folderName);
        string pathToSave = Path.Join(_basePath, folderName);

        Directory.CreateDirectory(pathToSave);

        string fileName = request.File.FileName.Trim('"');
        fileName = RemoveSpecialCharacters(fileName);
        fileName = fileName.ReplaceWhitespace("-");
        fileName += ext.Trim();
        string fullPath = Path.Join(pathToSave, fileName);
        string dbPath = Path.Join(folderName, fileName);
        if (File.Exists(dbPath))
        {
            dbPath = NextAvailableFilename(dbPath);
            fullPath = NextAvailableFilename(fullPath);
        }

        dbPath = dbPath.Replace("\\", "/");

        _logger.LogInformation(
            "[{ServiceName}] File path retrieved, path: \'{FullPath}\'",
            nameof(FileStorageService),
            fullPath
        );

        return "/" + dbPath;
    }

    /// <summary>
    ///     Allow upload/download/remove of files in queue to be processed later.
    /// </summary>
    /// <returns></returns>
    public FileStorageQueueManager InitiateQueueManager()
    {
        return new FileStorageQueueManager(this);
    }

    internal async Task<string> UploadAsync(
        Type type,
        FileUploadBase64String request,
        FileUtility.FileFormat[] supportedFileFormats,
        CancellationToken cancellationToken = default,
        byte[]? rawData = null
    )
    {
        if (request.Data.IsNullOrWhiteSpace() && rawData.IsNull())
        {
            throw new InvalidOperationException("File must not empty.");
        }

        if (!FileUtility.IsExtensionMatched(request.Extension, supportedFileFormats))
        {
            throw new InvalidOperationException("File format is not supported.");
        }

        if (request.Name.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException("Name is required.");
        }

        using MemoryStream streamData = _recyclableMemoryStreamManager.GetStream(
            rawData.IsNull() ? Convert.FromBase64String(request.Data) : rawData
        );

        if (streamData.Length > 0)
        {
            string folderName = GetFolderName(type, supportedFileFormats);
            string pathToSave = Path.Join(_basePath, folderName);

            Directory.CreateDirectory(pathToSave);

            string fileName = request.Name.Trim('"');
            fileName = RemoveSpecialCharacters(fileName);
            fileName = fileName.ReplaceWhitespace("-");
            fileName += request.Extension.Trim();
            string fullPath = Path.Join(pathToSave, fileName);
            string dbPath = Path.Join(folderName, fileName);
            if (File.Exists(dbPath))
            {
                dbPath = NextAvailableFilename(dbPath);
                fullPath = NextAvailableFilename(fullPath);
            }

            await using FileStream stream = new(fullPath, FileMode.Create);
            await streamData.CopyToAsync(stream, cancellationToken);
            dbPath = dbPath.Replace("\\", "/");

            _logger.LogInformation(
                "[{ServiceName}] File uploaded, path: \'{FullPath}\'",
                nameof(FileStorageService),
                fullPath
            );

            return "/" + dbPath;
        }

        _logger.LogError(
            "[{ServiceName}] File uploaded failed due to no stream data",
            nameof(FileStorageService)
        );

        return string.Empty;
    }

    internal async Task<string> UploadAsync(
        Type type,
        FileUploadForm request,
        FileUtility.FileFormat[] supportedFileFormats,
        CancellationToken cancellationToken = default
    )
    {
        if (request.File.Length.IsZero() || request.File.Length.IsNegative())
        {
            throw new InvalidOperationException("File must not empty.");
        }

        string ext = Path.GetExtension(request.File.FileName);

        if (!FileUtility.IsExtensionMatched(ext, supportedFileFormats))
        {
            throw new InvalidOperationException("File format is not supported.");
        }

        if (request.File.FileName.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException("Name is required.");
        }

        using MemoryStream streamData = _recyclableMemoryStreamManager.GetStream();
        await using Stream fileStream = request.File.OpenReadStream();
        await fileStream.CopyToAsync(streamData, cancellationToken);

        if (streamData.Length > 0)
        {
            string folderName = GetFolderName(type, supportedFileFormats);
            string pathToSave = Path.Join(_basePath, folderName);

            Directory.CreateDirectory(pathToSave);

            string fileName = request.File.FileName.Trim('"');
            fileName = RemoveSpecialCharacters(fileName);
            fileName = fileName.ReplaceWhitespace("-");
            fileName += ext.Trim();
            string fullPath = Path.Join(pathToSave, fileName);
            string dbPath = Path.Join(folderName, fileName);
            if (File.Exists(dbPath))
            {
                dbPath = NextAvailableFilename(dbPath);
                fullPath = NextAvailableFilename(fullPath);
            }

            await using FileStream stream = new(fullPath, FileMode.Create);
            await streamData.CopyToAsync(stream, cancellationToken);
            dbPath = dbPath.Replace("\\", "/");

            _logger.LogInformation(
                "[{ServiceName}] File uploaded, path: \'{FullPath}\'",
                nameof(FileStorageService),
                fullPath
            );

            return "/" + dbPath;
        }

        _logger.LogError(
            "[{ServiceName}] File uploaded failed due to no stream data",
            nameof(FileStorageService)
        );

        return string.Empty;
    }

    internal async Task RemoveAsync(
        Type type,
        string fileName,
        FileUtility.FileFormat fileFormat,
        CancellationToken cancellationToken = default
    )
    {
        await Task.CompletedTask;

        if (fileName.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException("File name must be provided.");
        }

        string folderName = GetFolderName(type, new[] { fileFormat });
        string pathToDelete = Path.Join(_basePath, folderName);

        string fullPath = Path.Join(pathToDelete, $"{fileName}{fileFormat.Extension}");

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);

            _logger.LogInformation(
                "[{ServiceName}] File removed, path: \'{FullPath}\'",
                nameof(FileStorageService),
                fullPath
            );

            return;
        }

        _logger.LogWarning(
            "[{ServiceName}] File removed skipped due to file does not exist, path: \'{FullPath}\'",
            nameof(FileStorageService),
            fullPath
        );
    }

    private static string RemoveSpecialCharacters(string str)
    {
        return Regex.Replace(str, "[^a-zA-Z0-9_.]+", string.Empty, RegexOptions.Compiled);
    }

    private static string NextAvailableFilename(string path)
    {
        if (!File.Exists(path))
        {
            return path;
        }

        if (Path.HasExtension(path))
        {
            return GetNextFilename(
                path.Insert(
                    path.LastIndexOf(Path.GetExtension(path), StringComparison.Ordinal),
                    NumberPattern
                )
            );
        }

        return GetNextFilename(path + NumberPattern);
    }

    private static string GetNextFilename(string pattern)
    {
        string tmp = string.Format(pattern, 1);

        if (!File.Exists(tmp))
        {
            return tmp;
        }

        int min = 1,
            max = 2;

        while (File.Exists(string.Format(pattern, max)))
        {
            min = max;
            max *= 2;
        }

        while (max != min + 1)
        {
            int pivot = (max + min) / 2;
            if (File.Exists(string.Format(pattern, pivot)))
            {
                min = pivot;
            }
            else
            {
                max = pivot;
            }
        }

        return string.Format(pattern, max);
    }

    private static string GetFolderName(
        Type type,
        FileUtility.FileFormat[] supportedFileFormats,
        string? subFolder = null
    )
    {
        string folder = type.Name;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            folder = folder.Replace(@"\", "/");
        }

        if (!subFolder.IsNull())
        {
            folder = Path.Join(subFolder, folder);
        }

        string folderName;

        if (
            FileUtility
                .GetStandardImageFileFormats()
                .Any(x => supportedFileFormats.Any(k => k == x))
        )
        {
            folderName = Path.Join("Files", "Images", folder);
        }
        else if (
            FileUtility
                .GetStandardExcelFileFormats()
                .Any(x => supportedFileFormats.Any(k => k == x))
            || supportedFileFormats.Any(
                x => x == FileUtility.GetPdfFileFormat() || x == FileUtility.GetTxtFileFormat()
            )
        )
        {
            folderName = Path.Join("Files", "Documents", folder);
        }
        else
        {
            folderName = Path.Join("Files", "Others", folder);
        }

        return folderName;
    }

    private static string GetFolderName(
        FileUtility.FileFormat[] supportedFileFormats,
        string folderName
    )
    {
        if (
            FileUtility
                .GetStandardImageFileFormats()
                .Any(x => supportedFileFormats.Any(k => k == x))
        )
        {
            return Path.Join("Files", "Images", folderName);
        }

        if (
            FileUtility
                .GetStandardExcelFileFormats()
                .Any(x => supportedFileFormats.Any(k => k == x))
            || supportedFileFormats.Any(
                x => x == FileUtility.GetPdfFileFormat() || x == FileUtility.GetTxtFileFormat()
            )
        )
        {
            return Path.Join("Files", "Documents", folderName);
        }

        return Path.Join("Files", "Others", folderName);
    }

    private static byte[] ConvertFileToByteArray(string fileName)
    {
        byte[]? fileData = null;

        using FileStream fs = File.OpenRead(fileName);
        BinaryReader binaryReader = new(fs);
        fileData = binaryReader.ReadBytes((int)fs.Length);

        return fileData;
    }

    public Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(global::Foundation.Features.DomainDrivenDesign.ValueObjects.ImageFile filePath, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(global::Foundation.Features.DomainDrivenDesign.ValueObjects.ImageFilePath filePath, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(global::Foundation.Features.DomainDrivenDesign.ValueObjects.TextFile filePath, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(global::Foundation.Features.DomainDrivenDesign.ValueObjects.TextFilePath filePath, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(global::Foundation.Features.DomainDrivenDesign.ValueObjects.ExcelFile filePath, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(global::Foundation.Features.DomainDrivenDesign.ValueObjects.ExcelFilePath filePath, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(global::Foundation.Features.DomainDrivenDesign.ValueObjects.File filePath, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(global::Foundation.Features.DomainDrivenDesign.ValueObjects.FilePath filePath, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
