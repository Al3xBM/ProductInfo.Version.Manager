using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using ProductInfo.Version.Manager.Services.IO.File;

namespace ProductInfo.Version.Manager.Services.Tests.VersionManagerServiceTests;

public class UpdateVersionForFeatureReleaseShould : VersionManagerServiceTestBase
{
    private readonly Func<Task<string>> _act;

    public UpdateVersionForFeatureReleaseShould()
    {
        _act = Service.UpdateVersionForFeatureRelease;
    }

    [Fact]
    public async Task ReturnUpdatedVersion_With_IncrementedMajorVersion()
    { 
        ++ProductInfo.MajorVersion;
        
        var result = await _act();

        result.Should().Be(ProductInfo.FullVersion);
        result.Split('.').Last().Should().Be("0");
    }

    [Fact]
    public async Task ReturnFeatureIncrementedDefaultVersion_When_FilePathNotSpecified()
    {
        var productInfoFileIo = new ProductInfoFileIo(
            NullLogger<ProductInfoFileIo>.Instance, 
            new ProductInfoFileConfig("dummy"), 
            FileSystem);
        var service = new VersionManagerService(Logger, productInfoFileIo);
        
        var result = await service.UpdateVersionForFeatureRelease();
        
        result.Should().Be(FeatureIncrementedDefaultVersion);
    }
}