using Microsoft.Extensions.Logging;
using ProductInfo.Version.Manager.Services.IO;

namespace ProductInfo.Version.Manager.Services;

public class VersionManagerService : IVersionManagerService
{
    public const string UpdatingVersionNumberForRelease = "Updating version number for {0} release";
    public const string UpdatedProductInfoVersion = "Updated ProductInfo version is : {0};";
    public const string CurrentProductInfoVersion = "Current ProductInfo version is: '{0}'";

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
        
        _logger.LogInformation(CurrentProductInfoVersion, currentVersion);
    }

    public async Task<string> UpdateVersionForFeatureRelease()
    {
        _logger.LogInformation(UpdatingVersionNumberForRelease, "feature");
        
        var updatedVersion = await _productInfoIo.UpdateProductInfoAsync(true);
        
        LogUpdatedProductVersion(updatedVersion);
        
        return updatedVersion;
    }

    public async Task<string> UpdateVersionForBugfixRelease()
    {
        _logger.LogInformation(UpdatingVersionNumberForRelease, "bugfix");
        
        var updatedVersion = await _productInfoIo.UpdateProductInfoAsync();

        LogUpdatedProductVersion(updatedVersion);
        
        return updatedVersion;
    }

    private void LogUpdatedProductVersion(string updatedVersion)
    {
        _logger.LogInformation(UpdatedProductInfoVersion, updatedVersion);
    }
}