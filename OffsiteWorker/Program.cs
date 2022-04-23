using OffsiteWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostBuilderContext, services) =>
    {
        // Dependency injections
        services.AddDatabase(hostBuilderContext);
        // Long running tasks
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
