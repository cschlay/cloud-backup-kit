using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Tests;

public static class TestConfiguration
{
    public static IConfiguration BuildConfiguration()
    {
        var builder = new ConfigurationBuilder();
        builder.AddInMemoryCollection(new Dictionary<string, string>
        {
            { "App:FileSystemRoot", TestConstants.FileDirectoryRoot }
        });

        return builder.Build();
    }
}
