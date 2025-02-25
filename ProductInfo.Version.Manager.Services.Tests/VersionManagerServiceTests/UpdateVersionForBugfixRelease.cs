using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using ProductInfo.Version.Manager.Services.IO.File;
using static ProductInfo.Version.Manager.Services.Tests.Common.Constants;

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
        FileSystem.RemoveFile(DefaultPath);
        
        var result = await _act();
        
        result.Should().Be(BugfixIncrementedDefaultVersion);
    }
}