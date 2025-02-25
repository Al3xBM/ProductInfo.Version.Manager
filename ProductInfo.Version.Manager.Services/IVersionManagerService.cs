namespace ProductInfo.Version.Manager.Services;

public interface IVersionManagerService
{
    Task CheckCurrentVersion();
    
    Task<string> UpdateVersionForFeatureRelease();

    Task<string> UpdateVersionForBugfixRelease();
}