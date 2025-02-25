using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductInfo.Version.Manager.Config;
using ProductInfo.Version.Manager.Handlers;
using ProductInfo.Version.Manager.Services;
using ProductInfo.Version.Manager.Services.IO;
using ProductInfo.Version.Manager.Services.IO.File;

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IInputHandler, InputHandler>();
        services.AddSingleton<IVersionManagerService, VersionManagerService>();
        services.AddSingleton<IProductInfoIo, ProductInfoFileIo>();
        services.AddSingleton(ProductInfoFileConfigProvider.GetConfig());
        services.AddSingleton<IFileSystem, FileSystem>();
    })
    .Build();
    
var handler = host.Services.GetRequiredService<IInputHandler>();

await handler.RunAsync(args);
