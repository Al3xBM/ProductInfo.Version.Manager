using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using ProductInfo.Version.Manager.Handlers;
using ProductInfo.Version.Manager.Services;
using ProductInfo.Version.Manager.Services.IO.File;
using static ProductInfo.Version.Manager.Tests.Common.Constants;

namespace ProductInfo.Version.Manager.Tests.InputHandlerTests;

public abstract class InputHandlerTestBase
{
    protected readonly InputHandler Service;
    protected readonly ILogger<InputHandler> Logger;
    protected readonly IVersionManagerService VersionManager;
    protected readonly MockFileSystem FileSystem;
    
    protected InputHandlerTestBase()
    {
        FileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { DefaultPath, new MockFileData(MockVersion) }
        });
        var productInfoIo = new ProductInfoFileIo(
            NullLogger<ProductInfoFileIo>.Instance, 
            new ProductInfoFileConfig(), 
            FileSystem);
        VersionManager = Substitute.ForPartsOf<VersionManagerService>(NullLogger<VersionManagerService>.Instance, productInfoIo);
        Logger = Substitute.For<ILogger<InputHandler>>();
        Service = new InputHandler(Logger, VersionManager, FileSystem);
    }
}