using Microsoft.Extensions.Logging;
using static System.IO.File;

namespace ProductInfo.Version.Manager.Services.IO.File;

public class ProductInfoFileIo : IProductInfoIo
{
    private readonly ILogger<ProductInfoFileIo> _logger;
    private readonly ProductInfoFileConfig _config;

    public ProductInfoFileIo(ILogger<ProductInfoFileIo> logger, ProductInfoFileConfig config)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<string> ReadProductInfoAsync()
    {
        EnsureFileExists();
        
        var text = await ReadAllTextAsync(_config.FilePath);

        return text;
    }

    public async Task UpdateProductInfoAsync()
    {
        EnsureFileExists();
        
        await Task.CompletedTask;
    }

    private void EnsureFileExists()
    {
        if (!Exists(_config.FilePath))
        {
            _logger.LogWarning("File does not exist. Will create a new file at the specified location.");
            WriteAllText(_config.FilePath, string.Empty);
        }
        else
        {
            _logger.LogInformation("File exists");
        }
    }
}