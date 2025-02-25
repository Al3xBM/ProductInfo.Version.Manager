using FluentAssertions;
using static ProductInfo.Version.Manager.Services.Tests.Common.Constants;

namespace ProductInfo.Version.Manager.Services.Tests.ProductInfoFileIoTests;

public class ReadProductInfoAsyncShould : ProductInfoFileIoTestsBase
{
    private readonly Func<Task<string>> _act;

    public ReadProductInfoAsyncShould()
    {
        _act = Service.ReadProductInfoAsync;
    }

    [Fact]
    public async Task ReturnActualVersion_When_FilePathIsSet()
    {
        var result = await _act();

        result.Should().Be(ProductInfo.FullVersion);
    }

    [Fact]
    public async Task ReturnDefaultVersion_When_FilePathIsNotSet()
    {
        FileSystem.RemoveFile(DefaultPath);
        
        var result = await _act();

        result.Should().Be(DefaultVersion);
    }
}