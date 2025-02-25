using ProductInfo.Version.Manager.Services.IO.File;

namespace ProductInfo.Version.Manager.Config;

public static class ProductInfoFileConfigProvider
{
    private static ProductInfoFileConfig _instance = new();
    
    public static ProductInfoFileConfig GetConfig()
    {
        return _instance;
    }

    public static void SwapInstance(ProductInfoFileConfig instance)
    {
        _instance = instance;
    }
}