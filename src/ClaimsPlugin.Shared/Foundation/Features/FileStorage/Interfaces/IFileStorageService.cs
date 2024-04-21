using ClaimsPlugin.Shared.Foundation.Common.Utilities;
using ClaimsPlugin.Shared.Foundation.Features.DependencyInjection.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.FileStorage.Helpers;
using ClaimsPlugin.Shared.Foundation.Features.FileStorage.Models;
using Foundation.Features.DomainDrivenDesign.ValueObjects;
using File = Foundation.Features.DomainDrivenDesign.ValueObjects.File;

namespace ClaimsPlugin.Shared.Foundation.Features.FileStorage.Interfaces;

public interface IFileStorageService : ITransientService
{
    Task<string> PushAsync<T>(
        FileUploadBase64String request,
        FileUtility.FileFormat supportedFileFormat,
        CancellationToken cancellationToken = default
    )
        where T : class;
    Task<string> PushAsync(
        FileUploadBase64String request,
        FileUtility.FileFormat supportedFileFormat,
        string folderName,
        CancellationToken cancellationToken = default
    );
    Task<string> PushAsync<T>(
        FileUploadBase64String request,
        FileUtility.FileFormat[] supportedFileFormats,
        CancellationToken cancellationToken = default
    )
        where T : class;
    Task<string> PushAsync(
        FileUploadBase64String request,
        FileUtility.FileFormat[] supportedFileFormats,
        string folderName,
        CancellationToken cancellationToken = default
    );
    Task<string> PushAsync<T>(
        FileUploadForm request,
        FileUtility.FileFormat supportedFileFormat,
        CancellationToken cancellationToken = default
    )
        where T : class;
    Task<string> PushAsync(
        FileUploadForm request,
        FileUtility.FileFormat supportedFileFormat,
        string folderName,
        CancellationToken cancellationToken = default
    );
    Task<string> PushAsync<T>(
        FileUploadForm request,
        FileUtility.FileFormat[] supportedFileFormats,
        CancellationToken cancellationToken = default
    )
        where T : class;
    Task<string> PushAsync(
        FileUploadForm request,
        FileUtility.FileFormat[] supportedFileFormats,
        string folderName,
        CancellationToken cancellationToken = default
    );
    Task<string> PushAsync<T>(
        byte[] rawData,
        string fileName,
        FileUtility.FileFormat fileFormat,
        CancellationToken cancellationToken = default
    )
        where T : class;
    Task<byte[]> PullAsByteArrayAsync<T>(
        string fileName,
        FileUtility.FileFormat fileFormat,
        CancellationToken cancellationToken = default
    )
        where T : class;
    Task<string> PullAsBase64StringAsync<T>(
        string fileName,
        FileUtility.FileFormat fileFormat,
        CancellationToken cancellationToken = default
    )
        where T : class;
    Task<byte[]> PullAsByteArrayAsync(
        string filePath,
        CancellationToken cancellationToken = default
    );
    Task<string> PullAsBase64StringAsync(
        string filePath,
        CancellationToken cancellationToken = default
    );
    Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(
        string filePath,
        CancellationToken cancellationToken = default
    );
    Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(
        ImageFile filePath,
        CancellationToken cancellationToken = default
    );
    Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(
        ImageFilePath filePath,
        CancellationToken cancellationToken = default
    );
    Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(
        TextFile filePath,
        CancellationToken cancellationToken = default
    );
    Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(
        TextFilePath filePath,
        CancellationToken cancellationToken = default
    );
    Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(
        ExcelFile filePath,
        CancellationToken cancellationToken = default
    );
    Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(
        ExcelFilePath filePath,
        CancellationToken cancellationToken = default
    );
    Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(
        File filePath,
        CancellationToken cancellationToken = default
    );
    Task<FileBase64StringDataTransferObject> PullAsFileBase64StringDataTransferObjectAsync(
        FilePath filePath,
        CancellationToken cancellationToken = default
    );
    Task<byte[]> PullAsByteArrayAsync(
        string fileName,
        FileUtility.FileFormat fileFormat,
        string folderName,
        CancellationToken cancellationToken = default
    );
    Task<string> PullAsBase64StringAsync(
        string fileName,
        FileUtility.FileFormat fileFormat,
        string folderName,
        CancellationToken cancellationToken = default
    );
    Task<string> GetFilePathAsync<T>(
        string fileName,
        FileUtility.FileFormat fileFormat,
        CancellationToken cancellationToken = default
    )
        where T : class;
    Task<string> GetFilePathAsync(
        string fileName,
        FileUtility.FileFormat fileFormat,
        string folderName,
        CancellationToken cancellationToken = default
    );
    Task DeleteAsync<T>(
        string fileName,
        FileUtility.FileFormat fileFormat,
        CancellationToken cancellationToken = default
    )
        where T : class;
    Task DeleteAsync(
        string fileName,
        FileUtility.FileFormat fileFormat,
        string folderName,
        CancellationToken cancellationToken = default
    );
    Task DeleteAsync(string filePath, CancellationToken cancellationToken = default);
    Task<string> GetPushPathAsync<T>(
        FileUploadBase64String request,
        FileUtility.FileFormat supportedFileFormat,
        CancellationToken cancellationToken = default
    )
        where T : class;
    Task<string> GetPushPathAsync(
        FileUploadBase64String request,
        FileUtility.FileFormat supportedFileFormat,
        string folderName,
        CancellationToken cancellationToken = default
    );
    Task<string> GetPushPathAsync<T>(
        FileUploadBase64String request,
        FileUtility.FileFormat[] supportedFileFormats,
        CancellationToken cancellationToken = default
    )
        where T : class;
    Task<string> GetPushPathAsync(
        FileUploadBase64String request,
        FileUtility.FileFormat[] supportedFileFormats,
        string folderName,
        CancellationToken cancellationToken = default
    );
    Task<string> GetPushPathAsync<T>(
        FileUploadForm request,
        FileUtility.FileFormat supportedFileFormat,
        CancellationToken cancellationToken = default
    )
        where T : class;
    Task<string> GetPushPathAsync(
        FileUploadForm request,
        FileUtility.FileFormat supportedFileFormat,
        string folderName,
        CancellationToken cancellationToken = default
    );
    Task<string> GetPushPathAsync<T>(
        FileUploadForm request,
        FileUtility.FileFormat[] supportedFileFormats,
        CancellationToken cancellationToken = default
    )
        where T : class;
    Task<string> GetPushPathAsync(
        FileUploadForm request,
        FileUtility.FileFormat[] supportedFileFormats,
        string folderName,
        CancellationToken cancellationToken = default
    );
    FileStorageQueueManager InitiateQueueManager();
}
