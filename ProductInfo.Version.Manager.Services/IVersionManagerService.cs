namespace ProductInfo.Version.Manager.Services;

public interface IVersionManagerService
{
    Task CheckCurrentVersion();
    
    Task UpdateVersionForFeatureRelease();

    Task UpdateVersionForBugfixRelease();
}