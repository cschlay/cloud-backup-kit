using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests;

public class TestBase
{
    protected readonly IConfiguration Configuration;
    
    protected TestBase()
    {
        var builder = new ConfigurationBuilder();
        builder.AddInMemoryCollection(new Dictionary<string, string>
        {
            { "App:FileSystemRoot", TestConstants.FileDirectoryRoot },
            { "App:CleanupDelay", "0" },
            { "ConnectionStrings:AzureBlobStorage", "UseDevelopmentStorage=true" }
        });

        Configuration = builder.Build();
    }

    protected static ILogger<T> MockLogger<T>() => new Mock<ILogger<T>>().Object;
}
