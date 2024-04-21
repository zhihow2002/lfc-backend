namespace ClaimsPlugin.Shared.Foundation.Features.FileStorage.Models;

public class FileStorageSettings
{
    public required AzureFileObject AzureFiles { get; set; }
    
    public class AzureFileObject
    {
        public required bool Enabled { get; set; }
        public required string Path { get; set; }
    }
}
