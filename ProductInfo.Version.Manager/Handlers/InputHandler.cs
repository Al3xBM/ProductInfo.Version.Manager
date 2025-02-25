using System.Globalization;
using Microsoft.Extensions.Logging;
using ProductInfo.Version.Manager.Config;
using ProductInfo.Version.Manager.Services;
using ProductInfo.Version.Manager.Services.IO.File;

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
        await ProcessInputArgs(args);
        
        await ProcessInputAsync();
    }

    private async Task ProcessInputArgs(string[] args)
    {
        if (args.Length == 0)
        {
            _logger.LogInformation("No arguments provided, using default ProductInfo file path.");
            return;
        }
        
        var argsList = args.ToList();

        ProcessFilePathArgs(argsList);
        await ProcessReleaseTypeArgs(argsList);

        Environment.Exit(0);
    }
    
    private async Task ProcessInputAsync()
    {
        SetFilePath();

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
            }
        }
    }

    private void SetFilePath()
    {
        while (true)
        {
            _logger.LogInformation("Please provide a ProductInfo file path:");
            var input = Console.ReadLine()?.Trim();

            if (File.Exists(input))
            {
                ProductInfoFileConfigProvider.SwapInstance(new ProductInfoFileConfig(input));
                return;
            }
            
            _logger.LogInformation("File does not exist.");
        }
    }
    
    private void ProcessFilePathArgs(List<string> argsList)
    {
        var path = ExtractValueFromArgs(argsList, "--path");
        if (!File.Exists(path))
        {
            _logger.LogError("File not found under specified path: '{Path}'", path);
            throw new ArgumentException();
        }
            
        ProductInfoFileConfigProvider.SwapInstance(new ProductInfoFileConfig(path));
    }

    private async Task ProcessReleaseTypeArgs(List<string> argsList)
    {
        var releaseType = ExtractValueFromArgs(argsList, "--releaseType");
        var releaseTypes = new List<string> {"bugfix", "feature"};
        if (!releaseTypes.Contains(releaseType))
        {
            _logger.LogError("Provided value is not a valid release type: '{ReleaseType}'", releaseType);
            throw new ArgumentException();
        }

        if (releaseType == "bugfix")
        {
            await _versionManagerService.UpdateVersionForBugfixRelease();
        }
        else
        {
            await _versionManagerService.UpdateVersionForFeatureRelease();
        }
    }

    private string ExtractValueFromArgs(List<string> args, string flag)
    {
        if (args.Count >= 2 && args.SkipLast(1).Contains(flag))
        {
            var pathFlagIndex = args.IndexOf(flag);
            var value = args[pathFlagIndex + 1].Trim('"');

            return value;
        }

        _logger.LogError("Expected arguments format is \"--path %path% --releaseType %releaseType%\"");
        throw new ArgumentException();
    }
}