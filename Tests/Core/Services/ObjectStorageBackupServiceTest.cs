using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Enums;
using Core.Exceptions;
using Core.Models;
using Core.Services;
using Core.Utils;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Tests.Core.Services;

public class ObjectStorageBackupServiceTest
{
    private readonly IConfiguration _configuration = TestConfiguration.BuildConfiguration();
    private const string DirectoryPath = $"{TestConstants.FileDirectoryRoot}/{TestConstants.StorageDirectory}";

    private readonly ObjectStorageFile _fixture;

    public ObjectStorageBackupServiceTest()
    {
        _fixture =  new ObjectStorageFile
        {
            Name = "chewing-gum.jpg",
            Path = $"books/{nameof(BackupFileAsyncTest)}",
            SignedDownloadUrl = "http://localhost:5000/object-storage/url",
            Status = SyncStatusEnum.Pending,
            Storage = TestConstants.StorageDirectory,
            Version = 1
        };
    }
    
    [Fact]
    public async Task BackupFileAsyncTest()
    {
        string content = $"test-{Guid.NewGuid()}";
        
        var mockHttpService = new Mock<IHttpService>();
        mockHttpService
            .Setup(m => m.OpenHttpStreamAsync(It.IsAny<string>()))
            .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));
        
        var service = new ObjectStorageBackupService(
            dbContext: new Mock<AppDbContext>().Object,
            configuration: _configuration,
            fileSanitizer: new FileSanitizer(),
            httpService: mockHttpService.Object);

        ObjectStorageFile file = await service.BackupFileAsync(_fixture);
        Assert.NotNull(file.SyncedAt);
        Assert.Equal(SyncStatusEnum.Completed, file.Status);

        var path = $"{DirectoryPath}/{file.Path.ToLower()}/v1.gzip";
        Assert.Equal(path, file.BackupLocation);
    }
    
    [Fact]
    public async Task BackupFileAsync404Test()
    {
        var mockHttpService = new Mock<IHttpService>();
        mockHttpService.Setup(m => m.OpenHttpStreamAsync(It.IsAny<string>())).Throws<HttpResourceDoesNotExist>();
        var service = new ObjectStorageBackupService(
            dbContext: new Mock<AppDbContext>().Object,
            configuration: _configuration,
            fileSanitizer: new FileSanitizer(),
            httpService: mockHttpService.Object);
        
        ObjectStorageFile file = await service.BackupFileAsync(_fixture);
        Assert.Equal(SyncStatusEnum.Deleted, file.Status);
    }


    [Fact]
    public async Task SaveFileAsyncTest()
    {
        string content = $"test-{Guid.NewGuid()}";
        var mockHttpService = new Mock<IHttpService>();
        mockHttpService
            .Setup(m => m.OpenHttpStreamAsync(It.IsAny<string>()))
            .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));
        
        var service = new ObjectStorageBackupService(
            dbContext: new Mock<AppDbContext>().Object,
            configuration: _configuration,
            fileSanitizer: new FileSanitizer(),
            httpService: mockHttpService.Object);

        const string filename = $"{nameof(SaveFileAsyncTest)}.txt.gzip";
        const string path = $"{DirectoryPath}/{filename}";
        await service.SaveFileAsync("/", DirectoryPath, filename);
        
        string savedContent = await FileTools.ReadGzipTextContentAsync(path);
        Assert.True(File.Exists(path));
        Assert.Equal(content, savedContent);
    }
}
