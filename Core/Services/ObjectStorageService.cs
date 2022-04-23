using Core.Integrations.ObjectStorage;
using Core.Models;
using Microsoft.Extensions.Configuration;

namespace Core.Services;

public class ObjectStorageService : IObjectStorageService
{
    private IObjectStorageProvider _provider;
        
    public ObjectStorageService(IConfiguration configuration)
    {
        _provider = GetProvider(configuration);
    }

    public Task<string> DownloadFileAsync(ObjectStorageFile file)
    {
        throw new NotImplementedException();
    }

    private IObjectStorageProvider GetProvider(IConfiguration configuration)
    {
        string provider = configuration["ObjectStorage:Provider"];
        if (provider == "Azure")
        {
            return new AzureBlobStorageProvider();
        }
        return new AwsS3Provider();
    }
}
