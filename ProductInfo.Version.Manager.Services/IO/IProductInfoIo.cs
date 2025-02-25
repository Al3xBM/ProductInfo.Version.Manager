namespace ProductInfo.Version.Manager.Services.IO;

public interface IProductInfoIo
{
    Task<string> ReadProductInfoAsync();

    Task<string> UpdateProductInfoAsync(bool isMajorRelease = false);
}