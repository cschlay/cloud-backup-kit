using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Core.Models;
using Core.Services;
using Core.Utils;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Tests.Core.Services;

public class ObjectStorageBackupServiceTest
{
    [Fact]
    public async Task BackupFileAsyncTest()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>())
            .Build();
        
        var mockDb = new Mock<AppDbContext>();
        var service = new ObjectStorageBackupService(mockDb.Object, new FileSanitizer(), new ObjectStorageService(configuration));
        
        var file = new ObjectStorageFile
        {
            Id = 1,
            Path = "/test/file"
        };
        await service.BackupFileAsync(file);
    }
}