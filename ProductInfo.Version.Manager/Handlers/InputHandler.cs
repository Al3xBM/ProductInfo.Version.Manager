using System.Globalization;
using Microsoft.Extensions.Logging;
using ProductInfo.Version.Manager.Services;

namespace ProductInfo.Version.Manager.Handlers;

public class InputHandler : IInputHandler
{
    private readonly ILogger<InputHandler> _logger;
    private readonly IVersionManagerService _versionManagerService;
    
    public InputHandler(ILogger<InputHandler> logger, IVersionManagerService versionManager)
    {
        _logger = logger;
        _versionManagerService = versionManager;
    }
    
    public async Task RunAsync(string[] args)
    {
        // TODO : actual processing for input args
        if (args.Length == 0)
        {
            _logger.LogInformation("No arguments provided, using default ProductInfo file path.");
        }
        
        await _versionManagerService.CheckCurrentVersion();
        
        while (true)
        {
            _logger.LogInformation("Enter command: ");
            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
                continue;

            switch (input.Trim().ToLower(CultureInfo.InvariantCulture))
            {
                case "exit":
                    _logger.LogInformation("Exiting...");
                    return;
                case "bugfix":
                    await _versionManagerService.UpdateVersionForBugfixRelease();
                    break;
                case "feature":
                    await _versionManagerService.UpdateVersionForFeatureRelease();
                    break;
                default:
                    break;
            }
        }
    }
}