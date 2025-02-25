using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using ProductInfo.Version.Manager.Services.IO;
using ProductInfo.Version.Manager.Services.IO.File;
using static ProductInfo.Version.Manager.Services.Tests.Common.Constants;

namespace ProductInfo.Version.Manager.Services.Tests.VersionManagerServiceTests;

public abstract class VersionManagerServiceTestBase
{
    protected readonly Models.ProductInfo ProductInfo;
    protected readonly VersionManagerService Service;
    protected readonly ILogger<VersionManagerService> Logger;
    protected readonly IProductInfoIo ProductInfoIo;
    protected readonly MockFileSystem FileSystem;
    
    protected VersionManagerServiceTestBase()
    {
        ProductInfo = new Models.ProductInfo
        {
            FullVersion = MockVersion
        };
        
        FileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { DefaultPath, new MockFileData(MockVersion) }
        });
        ProductInfoIo = new ProductInfoFileIo(
            NullLogger<ProductInfoFileIo>.Instance, 
            new ProductInfoFileConfig(), 
            FileSystem);
        Logger = Substitute.For<ILogger<VersionManagerService>>();
        Service = new VersionManagerService(Logger, ProductInfoIo);
    }
}