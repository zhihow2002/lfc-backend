using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Shared.Services
{
    internal class FileService : IFileService
    
    {
         private readonly FileStorageOptions _options;

        public FileService(IOptions<FileStorageOptions> options)
        {
            _options = options.Value;
        }

        public Task DeleteFileAsync(string key, CancellationToken cancellationToken = default)
        {
            var filePath = Path.Combine(_options.BasePath, key);

            if (Directory.Exists(filePath))
            {
                Directory.Delete(filePath, true);
            }
            return Task.CompletedTask;
        }

        public Task<bool> AnyAsync(string key, string documentName, CancellationToken cancellationToken = default)
        {
            var filePath = Path.Combine(_options.BasePath, key);
            return Task.FromResult(File.Exists(Path.Combine(filePath, documentName)));
        }

        public Task<bool> AnyAsync(string filePath, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(File.Exists(filePath));
        }

        public async Task<string> StoreFileAsync(DocumentDto document, CancellationToken cancellationToken = default)
        {
            var filePath = Path.Combine(_options.BasePath, document.Id.ToString());
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var fileName = Path.Combine(filePath, document.FileName);
            await File.WriteAllBytesAsync(fileName, document.Content, cancellationToken);
            return fileName;
        }

        public async Task StoreFilesAsync(List<DocumentDto> documents, CancellationToken cancellationToken = default)
        {
            var taskList = new List<Task<string>>();
            foreach (var document in documents)
            {
                taskList.Add(StoreFileAsync(document, cancellationToken));
            }

            await Task.WhenAll(taskList.ToArray());
        }
    }
}