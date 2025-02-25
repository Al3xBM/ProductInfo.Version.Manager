using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ProductInfo.Version.Manager.Services.IO.File;
using static ProductInfo.Version.Manager.Services.Tests.Common.Constants;

namespace ProductInfo.Version.Manager.Services.Tests.ProductInfoFileIoTests;

public abstract class ProductInfoFileIoTestsBase
{
    protected readonly ProductInfoFileIo Service;
    protected readonly ILogger<ProductInfoFileIo> Logger;
    protected readonly MockFileSystem FileSystem;
    protected readonly Models.ProductInfo ProductInfo;

    protected ProductInfoFileIoTestsBase()
    {
        ProductInfo = new Models.ProductInfo
        {
            FullVersion = MockVersion
        };
        
        Logger = Substitute.For<ILogger<ProductInfoFileIo>>();
        FileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { DefaultPath, new MockFileData(MockVersion) }
        });
        Service = new ProductInfoFileIo(Logger, new ProductInfoFileConfig(), FileSystem);
    }
}