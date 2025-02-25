using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using ProductInfo.Version.Manager.Services.IO;
using ProductInfo.Version.Manager.Services.IO.File;

namespace ProductInfo.Version.Manager.Services.Tests.VersionManagerServiceTests;

public abstract class VersionManagerServiceTestBase
{
    protected const string MockVersion = "1.2.3.4";
    protected const string DefaultVersion = "0.0.0.0";
    protected const string FeatureIncrementedDefaultVersion = "0.0.1.0";
    protected const string BugfixIncrementedDefaultVersion = "0.0.0.1";

    protected readonly Models.ProductInfo ProductInfo;
    protected readonly VersionManagerService Service;
    protected readonly ILogger<VersionManagerService> Logger;
    protected readonly IProductInfoIo ProductInfoIo;
    protected readonly IFileSystem FileSystem;
    
    protected VersionManagerServiceTestBase()
    {
        ProductInfo = new Models.ProductInfo
        {
            FullVersion = MockVersion
        };
        
        FileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { "ProductInfo.cs", new MockFileData(MockVersion) }
        });
        ProductInfoIo = new ProductInfoFileIo(
            NullLogger<ProductInfoFileIo>.Instance, 
            new ProductInfoFileConfig(), 
            FileSystem);
        Logger = Substitute.For<ILogger<VersionManagerService>>();
        Service = new VersionManagerService(Logger, ProductInfoIo);
    }
}