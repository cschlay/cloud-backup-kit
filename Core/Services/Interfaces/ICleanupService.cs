namespace Core.Services.Interfaces;

public interface ICleanupService
{
    public Task CleanupObjectStorageBackupAsync();
}