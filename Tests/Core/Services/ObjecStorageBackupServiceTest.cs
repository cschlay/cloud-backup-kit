﻿using System;
using System.Threading.Tasks;
using Core;
using Core.Models;
using Core.Services;
using Core.Utils;
using Moq;
using Xunit;

namespace Tests.Core.Services;

public class ObjecStorageBackupServiceTest
{
    [Fact]
    public async Task BackupFileAsyncTest()
    {
        var mockDb = new Mock<AppDbContext>();
        var service = new ObjectStorageBackupService(mockDb.Object, new FileSanitizer(), new S3ObjectStorageService());
        
        var file = new ObjectStorageFile
        {
            Id = 1,
            Path = "/test/file"
        };
        await service.BackupFileAsync(file);
    }
}