using Core.Utils;
using OffsiteWorker;
using OffsiteWorker.Workers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostBuilderContext, services) =>
    {
        services.AddHttpClient();
        // Dependency injections
        services.AddDatabase(hostBuilderContext);
        services.AddTransient<IFileSanitizer, FileSanitizer>();
        // Long running tasks
        services.AddHostedService<ObjectStorageSyncWorker>();
    })
    .Build();

await host.RunAsync();
