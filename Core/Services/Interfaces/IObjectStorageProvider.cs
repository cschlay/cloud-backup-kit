using Core.Models;

namespace Core.Services.Interfaces;

public interface IObjectStorageProvider
{
    public Task RestoreFile(ObjectStorageFile file);
}
