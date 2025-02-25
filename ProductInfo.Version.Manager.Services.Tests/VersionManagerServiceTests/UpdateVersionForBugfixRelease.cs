using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using ProductInfo.Version.Manager.Services.IO.File;

namespace ProductInfo.Version.Manager.Services.Tests.VersionManagerServiceTests;

public class UpdateVersionForBugfixRelease : VersionManagerServiceTestBase
{
    private readonly Func<Task<string>> _act;

    public UpdateVersionForBugfixRelease()
    {
        _act = Service.UpdateVersionForBugfixRelease;
    }
    
    [Fact]
    public async Task ReturnUpdatedVersion_With_IncrementedMinorVersion()
    { 
        ++ProductInfo.MinorVersion;
        
        var result = await _act();

        result.Should().Be(ProductInfo.FullVersion);
    }

    [Fact]
    public async Task ReturnBugfixIncrementedDefaultVersion_When_FilePathNotSpecified()
    {
        var productInfoFileIo = new ProductInfoFileIo(
            NullLogger<ProductInfoFileIo>.Instance, 
            new ProductInfoFileConfig("dummy"), 
            FileSystem);
        var service = new VersionManagerService(Logger, productInfoFileIo);
        
        var result = await service.UpdateVersionForBugfixRelease();
        
        result.Should().Be(BugfixIncrementedDefaultVersion);
    }
}