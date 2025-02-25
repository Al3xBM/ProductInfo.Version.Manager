using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using ProductInfo.Version.Manager.Services.IO.File;

namespace ProductInfo.Version.Manager.Services.Tests.VersionManagerServiceTests;

public class CheckCurrentVersionShould : VersionManagerServiceTestBase
{
    private readonly Func<Task> _act;

    public CheckCurrentVersionShould()
    {
        _act = Service.CheckCurrentVersion;
    }

    [Fact]
    public async Task LogActualVersion_When_FilePathIsSet()
    {
        await _act();
        
        CheckLogCall(
            string.Format(VersionManagerService.CurrentProductInfoVersion, MockVersion), 
            LogLevel.Information);
    }

    [Fact]
    public async Task LogDefaultVersion_When_FilePathNotSet()
    {
        var productInfoFileIo = new ProductInfoFileIo(
            NullLogger<ProductInfoFileIo>.Instance, 
            new ProductInfoFileConfig("dummy"), 
            FileSystem);
        var service = new VersionManagerService(Logger, productInfoFileIo);
        
        await service.CheckCurrentVersion();
        
        CheckLogCall(
            string.Format(VersionManagerService.CurrentProductInfoVersion, DefaultVersion), 
            LogLevel.Information);
    }

    private void CheckLogCall(string message, LogLevel logLevel)
    {
        Logger
            .Received(1)
            .Log(
                logLevel,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString() == message),
                null,
                Arg.Any<Func<object, Exception, string>>()!);
    }
}