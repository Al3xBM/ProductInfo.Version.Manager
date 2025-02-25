using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using ProductInfo.Version.Manager.Services.IO.File;
using static ProductInfo.Version.Manager.Services.Tests.Common.Constants;

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
        FileSystem.RemoveFile(DefaultPath);
        
        await _act();
        
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