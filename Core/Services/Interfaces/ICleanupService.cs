namespace Core.Services.Interfaces;

public interface ICleanupService
{
    public Task CleanupAsync();
    public Task CleanupObjectStorageBackupAsync();
}