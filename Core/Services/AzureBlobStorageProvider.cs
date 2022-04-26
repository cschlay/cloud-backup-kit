using Core.Models;
using Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Core.Services;

public class AzureBlobStorageProvider : IObjectStorageProvider
{
    private readonly IConfiguration _configuration;
    
    public AzureBlobStorageProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public Task RestoreFile(ObjectStorageFile file)
    {
        throw new NotImplementedException();
    }
}