using Core.Services;
using Core.Utils;
using OffsiteWorker;
using OffsiteWorker.Workers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostBuilderContext, services) =>
    {
        // Dependency injections
        services.AddDatabase(hostBuilderContext);
        services.AddTransient<IFileSanitizer, FileSanitizer>();
        services.AddTransient<IObjectStorageService, ObjectStorageService>();
        // Long running tasks
        services.AddHostedService<ObjectStorageSyncWorker>();
    })
    .Build();

await host.RunAsync();
