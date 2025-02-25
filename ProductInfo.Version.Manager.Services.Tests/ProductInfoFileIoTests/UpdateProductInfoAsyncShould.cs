using FluentAssertions;
using static ProductInfo.Version.Manager.Services.Tests.Common.Constants;

namespace ProductInfo.Version.Manager.Services.Tests.ProductInfoFileIoTests;

public class UpdateProductInfoAsyncShould : ProductInfoFileIoTestsBase
{
    private readonly Func<bool, Task<string>> _act;

    public UpdateProductInfoAsyncShould()
    {
        _act = Service.UpdateProductInfoAsync;
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ReturnUpdatedActualVersion_When_FilePathIsSet(bool isMajorRelease)
    {
        if (isMajorRelease)
        {
            ++ProductInfo.MajorVersion;
        }
        else
        {
            ++ProductInfo.MinorVersion;
        }
        
        var result = await _act(isMajorRelease);
        
        result.Should().Be(ProductInfo.FullVersion);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ReturnUpdatedDefaultVersion_When_FilePathIsNotSet(bool isMajorRelease)
    {
        FileSystem.RemoveFile(DefaultPath);

        var result = await _act(isMajorRelease);
        
        result.Should().Be(isMajorRelease 
            ? FeatureIncrementedDefaultVersion 
            : BugfixIncrementedDefaultVersion);
    }
}