using System;
using System.IO;
using System.Threading.Tasks;
using Core.Enums;
using Core.Models;
using Core.Services;
using Xunit;

namespace Tests.Core.Services;

public class CleanupServiceTest : DatabaseTestBase
{
    private const string DirectoryPath = $"{TestConstants.FileDirectoryRoot}/CleanupTest";
    private readonly CleanupService _service;
    
    public CleanupServiceTest()
    {
        Directory.CreateDirectory(DirectoryPath);
        _service = new CleanupService(
            dbContext: DbContext, 
            configuration: Configuration,
            logger: MockLogger<CleanupService>());
    }
    
    [Fact]
    public async Task CleanupObjectStorageBackupAsyncTest()
    {
        const string path = $"{DirectoryPath}/v3";
        FileStream fileStream = File.Create(path);
        fileStream.Close();

        ObjectStorageDeleteLog fixture = await CreateFixtureAsync(path);
        await _service.CleanupObjectStorageBackupAsync();
        Assert.False(File.Exists(path));
        Assert.NotNull(fixture.ConfirmedAt);
        Assert.Equal(SyncStatusEnum.Deleted, fixture.File!.Status);
    }

    [Fact]
    public async Task CleanupFileDoesNotExist()
    {
        const string path = $"{DirectoryPath}/void/not-found/v3";
        ObjectStorageDeleteLog fixture = await CreateFixtureAsync(path);
        Assert.False(File.Exists(path));
        await _service.CleanupObjectStorageBackupAsync();
        await DbContext.Entry(fixture).ReloadAsync();
        Assert.NotNull(fixture.ConfirmedAt);
    }

    private async Task<ObjectStorageDeleteLog> CreateFixtureAsync(string path)
    {
        var file = new ObjectStorageFile
        {
            Name = "CleanupTest.txt",
            Path = "tests/CleanupTest.txt",
            BackupLocation = path,
            Status = SyncStatusEnum.Completed,
            SyncedAt = DateTime.UtcNow,
        };
        var log = new ObjectStorageDeleteLog
        {
            DeletedAt = DateTime.UtcNow,
            File = file
        };
        DbContext.ObjectStorageDeleteLogs.Add(log);
        await DbContext.SaveChangesAsync();

        return log;
    }
}
