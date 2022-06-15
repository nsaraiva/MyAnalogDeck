using MyAnalogDeck.WorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<MyAnalogDeckWorker>();
    })
    .Build();

await host.RunAsync();
