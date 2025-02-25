using System.IO.Abstractions;
using Microsoft.Extensions.Logging;

namespace ProductInfo.Version.Manager.Services.IO.File;

public class ProductInfoFileIo : IProductInfoIo
{
    private readonly ILogger<ProductInfoFileIo> _logger;
    private readonly ProductInfoFileConfig _config;
    private readonly IFileSystem _fileSystem;
    private Models.ProductInfo? _productInfo;

    public ProductInfoFileIo(ILogger<ProductInfoFileIo> logger, ProductInfoFileConfig config, IFileSystem fileSystem)
    {
        _logger = logger;
        _config = config;
        _fileSystem = fileSystem;
    }

    public async Task<string> ReadProductInfoAsync()
    {
        EnsureFileExists();
        
        var text = await _fileSystem.File.ReadAllLinesAsync(_config.FilePath);
        
        if (_productInfo == default)
        {
            _productInfo = new Models.ProductInfo
            {
                AdditionalInfo = text.Skip(1),
                FullVersion = text.First()
            };
        }

        return _productInfo.FullVersion;
    }

    public async Task<string> UpdateProductInfoAsync(bool isMajorRelease = false)
    {
        if (_productInfo == null)
        {
            _ = await ReadProductInfoAsync();
        }

        if (isMajorRelease)
        {
            ++_productInfo!.MajorVersion;
        }
        else
        {
            ++_productInfo!.MinorVersion;
        }
        
        await _fileSystem.File.WriteAllLinesAsync(
            _config.FilePath,
            _productInfo.AdditionalInfo.Prepend(_productInfo.FullVersion));
        
        return _productInfo.FullVersion;
    }

    private void EnsureFileExists()
    {
        if (_fileSystem.File.Exists(_config.FilePath))
        {
            return;
        }

        _fileSystem.File.WriteAllText(_config.FilePath, "0.0.0.0");
        _logger.LogWarning("File does not exist. Created a new file at the specified location: {FilePath}",
            _config.FilePath);
    }
}