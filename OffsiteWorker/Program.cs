using Core.Services;
using Core.Services.Interfaces;
using Core.Utils;
using OffsiteWorker;
using OffsiteWorker.Workers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostBuilderContext, services) =>
    {
        services.AddHttpClient();
        // Dependency injections
        services.AddDatabase(hostBuilderContext);
        services.AddTransient<ICleanupService, CleanupService>();
        services.AddTransient<IFileSanitizer, FileSanitizer>();
        services.AddTransient<IHttpService, HttpService>();
        services.AddTransient<IObjectStorageBackupService, ObjectStorageBackupService>();
        // Long running tasks
        services.AddHostedService<ObjectStorageSyncWorker>();
    })
    .Build();

await host.RunAsync();
