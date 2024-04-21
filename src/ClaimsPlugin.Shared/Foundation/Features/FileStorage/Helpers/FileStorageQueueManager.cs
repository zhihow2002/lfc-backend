using ClaimsPlugin.Shared.Foundation.Common.Utilities;
using ClaimsPlugin.Shared.Foundation.Features.FileStorage.Models;
using ClaimsPlugin.Shared.Foundation.Features.FileStorage.Services;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
namespace ClaimsPlugin.Shared.Foundation.Features.FileStorage.Helpers;

public class FileStorageQueueManager
{
    private readonly FileStorageService _fileStorageService;
    private readonly List<string> _filePathToDelete;
    private readonly List<(Type type, string fileName, FileUtility.FileFormat fileFormat)> _filePathWithFileFormatToDelete;
    private readonly List<(string fileName, FileUtility.FileFormat fileFormat, string folderName)> _filePathWithFileFormatAndFolderNameToDelete;
    private readonly List<(Type type, FileUploadBase64String file, FileUtility.FileFormat[] supportedFileFormats)> _fileUploadBase64StringsToUpload;
    private readonly List<(FileUploadBase64String file, FileUtility.FileFormat[] supportedFileFormats, string folderName)> _fileUploadBase64StringsAndFolderNameToUpload;
    private readonly List<(Type type, FileUploadForm file, FileUtility.FileFormat[] supportedFileFormats)> _fileUploadFormsToUpload;
    private readonly List<(FileUploadForm file, FileUtility.FileFormat[] supportedFileFormats, string folderName)> _fileUploadFormsAndFolderNameToUpload;
    public FileStorageQueueManager(FileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
        _fileUploadBase64StringsToUpload = new List<(Type type, FileUploadBase64String file, FileUtility.FileFormat[] supportedFileFormats)>();
        _fileUploadFormsToUpload = new List<(Type type, FileUploadForm file, FileUtility.FileFormat[] supportedFileFormats)>();
        _filePathToDelete = new List<string>();
        _filePathWithFileFormatToDelete = new List<(Type type, string fileName, FileUtility.FileFormat fileFormat)>();
        _filePathWithFileFormatAndFolderNameToDelete = new List<(string fileName, FileUtility.FileFormat fileFormat, string folderName)>();
        _fileUploadBase64StringsAndFolderNameToUpload =
            new List<(FileUploadBase64String file, FileUtility.FileFormat[] supportedFileFormats, string folderName)>();
        _fileUploadFormsAndFolderNameToUpload = new List<(FileUploadForm file, FileUtility.FileFormat[] supportedFileFormats, string folderName)>();
    }
    public FileStorageQueueManager AddToUploadQueue<T>(FileUploadBase64String? request, FileUtility.FileFormat[] supportedFileFormats)
    {
        if (request.IsNotNull())
        {
            _fileUploadBase64StringsToUpload.Add((typeof(T), request, supportedFileFormats));
        }
        return this;
    }
    public FileStorageQueueManager AddToUploadQueue(FileUploadBase64String? request, FileUtility.FileFormat[] supportedFileFormats, string folderName)
    {
        if (request.IsNotNull())
        {
            _fileUploadBase64StringsAndFolderNameToUpload.Add((request, supportedFileFormats, folderName));
        }
        return this;
    }
    public FileStorageQueueManager AddToUploadQueue<T>(FileUploadBase64String? request, FileUtility.FileFormat supportedFileFormat)
    {
        if (request.IsNotNull())
        {
            _fileUploadBase64StringsToUpload.Add((typeof(T), request, new[] {supportedFileFormat}));
        }
        return this;
    }
    public FileStorageQueueManager AddToUploadQueue(FileUploadBase64String? request, FileUtility.FileFormat supportedFileFormat, string folderName)
    {
        if (request.IsNotNull())
        {
            _fileUploadBase64StringsAndFolderNameToUpload.Add((request, new[] {supportedFileFormat}, folderName));
        }
        return this;
    }
    public FileStorageQueueManager AddToUploadQueue<T>(FileUploadForm? request, FileUtility.FileFormat[] supportedFileFormats)
    {
        if (request.IsNotNull())
        {
            _fileUploadFormsToUpload.Add((typeof(T), request, supportedFileFormats));
        }
        return this;
    }
    public FileStorageQueueManager AddToUploadQueue(FileUploadForm? request, FileUtility.FileFormat[] supportedFileFormats, string folderName)
    {
        if (request.IsNotNull())
        {
            _fileUploadFormsAndFolderNameToUpload.Add((request, supportedFileFormats, folderName));
        }
        return this;
    }
    public FileStorageQueueManager AddToUploadQueue<T>(FileUploadForm? request, FileUtility.FileFormat supportedFileFormat)
    {
        if (request.IsNotNull())
        {
            _fileUploadFormsToUpload.Add((typeof(T), request, new[] {supportedFileFormat}));
        }
        return this;
    }
    public FileStorageQueueManager AddToUploadQueue(FileUploadForm? request, FileUtility.FileFormat supportedFileFormat, string folderName)
    {
        if (request.IsNotNull())
        {
            _fileUploadFormsAndFolderNameToUpload.Add((request, new[] {supportedFileFormat}, folderName));
        }
        return this;
    }
    public FileStorageQueueManager AddToDeleteQueue(string? filePath)
    {
        if (filePath.IsNotNullOrWhiteSpace())
        {
            _filePathToDelete.Add(filePath);
        }
        return this;
    }
    public FileStorageQueueManager AddToDeleteQueue<T>(string? fileName, FileUtility.FileFormat fileFormat)
    {
        if (fileName.IsNotNullOrWhiteSpace())
        {
            _filePathWithFileFormatToDelete.Add((typeof(T), fileName, fileFormat));
        }
        return this;
    }
    public FileStorageQueueManager AddToDeleteQueue(string? fileName, FileUtility.FileFormat fileFormat, string folderName)
    {
        if (fileName.IsNotNullOrWhiteSpace())
        {
            _filePathWithFileFormatAndFolderNameToDelete.Add((fileName, fileFormat, folderName));
        }
        return this;
    }
    public async Task RunAsync()
    {
        foreach (string filePath in _filePathToDelete)
        {
            await _fileStorageService.DeleteAsync(filePath);
        }
        foreach ((Type type, string fileName, FileUtility.FileFormat fileFormat) remove in _filePathWithFileFormatToDelete)
        {
            await _fileStorageService.RemoveAsync(remove.type, remove.fileName, remove.fileFormat);
        }
        foreach ((string fileName, FileUtility.FileFormat fileFormat, string folderName) remove in _filePathWithFileFormatAndFolderNameToDelete)
        {
            await _fileStorageService.DeleteAsync(remove.fileName, remove.fileFormat, remove.folderName);
        }
        foreach ((Type type, FileUploadBase64String file, FileUtility.FileFormat[] supportedFileFormats) upload in _fileUploadBase64StringsToUpload)
        {
            await _fileStorageService.UploadAsync(upload.type, upload.file, upload.supportedFileFormats.ToArray());
        }
        foreach ((FileUploadBase64String file, FileUtility.FileFormat[] supportedFileFormats, string folderName) upload in _fileUploadBase64StringsAndFolderNameToUpload)
        {
            await _fileStorageService.PushAsync(upload.file, upload.supportedFileFormats, upload.folderName);
        }
        foreach ((Type type, FileUploadForm file, FileUtility.FileFormat[] supportedFileFormats) upload in _fileUploadFormsToUpload)
        {
            await _fileStorageService.UploadAsync(upload.type, upload.file, upload.supportedFileFormats.ToArray());
        }
        foreach ((FileUploadForm file, FileUtility.FileFormat[] supportedFileFormats, string folderName) upload in _fileUploadFormsAndFolderNameToUpload)
        {
            await _fileStorageService.PushAsync(upload.file, upload.supportedFileFormats.ToArray(), upload.folderName);
        }
    }
}
