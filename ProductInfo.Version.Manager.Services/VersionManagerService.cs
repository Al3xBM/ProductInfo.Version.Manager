using Microsoft.Extensions.Logging;
using ProductInfo.Version.Manager.Services.IO;

namespace ProductInfo.Version.Manager.Services;

public class VersionManagerService : IVersionManagerService
{
    private const string UpdatingVersionNumberForRelease = "Updating version number for {ReleaseType} release";
    private readonly ILogger<VersionManagerService> _logger;
    private readonly IProductInfoIo _productInfoIo;


    public VersionManagerService(ILogger<VersionManagerService> logger, IProductInfoIo productInfoIo)
    {
        _logger = logger;
        _productInfoIo = productInfoIo;
    }

    public async Task CheckCurrentVersion()
    {
        var currentVersion = await _productInfoIo.ReadProductInfoAsync();
        
        _logger.LogInformation("Current ProductInfo version is : {CurrentVersion}", currentVersion);
    }

    public async Task UpdateVersionForFeatureRelease()
    {
        _logger.LogInformation(UpdatingVersionNumberForRelease, "feature");
        
        await Task.CompletedTask;
    }

    public async Task UpdateVersionForBugfixRelease()
    {
        _logger.LogInformation(UpdatingVersionNumberForRelease, "bugfix");
        
        await Task.CompletedTask;
    }

    private async Task UpdateMajorVersionNumber()
    {
        UpdateMinorVersionNumber(0);
    }

    private async Task UpdateMinorVersionNumber(int? version = null)
    {
        
    }
}