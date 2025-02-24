using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductInfo.Version.Manager.Services;

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IVersionManagerService, VersionManagerService>();
    })
    .Build();
    
await host.RunAsync();