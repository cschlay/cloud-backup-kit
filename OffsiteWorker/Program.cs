using OffsiteWorker;
using OffsiteWorker.Workers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostBuilderContext, services) =>
    {
        // Dependency injections
        services.AddDatabase(hostBuilderContext);
        // Long running tasks
        services.AddHostedService<ObjectStorageSyncWorker>();
    })
    .Build();

await host.RunAsync();
