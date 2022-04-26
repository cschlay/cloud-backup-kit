using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Core.Models;
using Core.Services.Interfaces;
using Core.Utils;
using Microsoft.Extensions.Configuration;

namespace Core.Services;

public class AzureBlobStorageProvider : IObjectStorageProvider
{
    private readonly IConfiguration _configuration;
    private BlobContainerClient? _container;
    
    public AzureBlobStorageProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task RestoreFileAsync(ObjectStorageFile file)
    {
        if (_container == null)
        {
            _container = new BlobContainerClient(_configuration.GetAzureBlobStorageKey(), file.Storage);
        } else if (_container.Name != file.Storage)
        {
            throw new ArgumentException("Storage differs, the database query should have limited the storage name.", nameof(file));
        }

        await using FileStream fileStream = File.OpenRead(file.BackupLocation);
        
        BlobClient blobClient = _container.GetBlobClient(file.Path);
        await blobClient.UploadAsync(fileStream, new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = file.ContentType
            }
        });
    }
}
