using Microsoft.Extensions.Logging.Abstractions;
using ProductInfo.Version.Manager.Services.IO;
using ProductInfo.Version.Manager.Services.IO.File;

namespace ProductInfo.Version.Manager.Services.Tests.VersionManagerServiceTests;

public abstract class VersionManagerServiceTestBase
{
    protected readonly VersionManagerService Service;
    protected readonly IProductInfoIo ProductInfoIo;
    
    protected VersionManagerServiceTestBase()
    {
        ProductInfoIo = new ProductInfoFileIo(NullLogger<ProductInfoFileIo>.Instance, new ProductInfoFileConfig());
        Service = new VersionManagerService(NullLogger<VersionManagerService>.Instance, ProductInfoIo);
    }
}