using System.Globalization;
using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using ProductInfo.Version.Manager.Config;
using ProductInfo.Version.Manager.Services;
using ProductInfo.Version.Manager.Services.IO.File;
using static ProductInfo.Version.Manager.Common.Constants;

namespace ProductInfo.Version.Manager.Handlers;

public class InputHandler : IInputHandler
{
    public const string FileNotFound = "File not found under specified path: '{0}'";
    public const string InvalidReleaseTypeValue = "Provided value is not a valid release type: '{0}'";
    public const string UnexpectedArgumentsFormat = $"Expected arguments format is \"{PathFlag} %path% {ReleaseTypeFlag} %releaseType%\"";
    public const string ProvidePath = "Please provide a ProductInfo file path:";
    public const string Exiting = "Exiting...";
    
    private readonly ILogger<InputHandler> _logger;
    private readonly IVersionManagerService _versionManagerService;
    private readonly IFileSystem _fileSystem;
    
    public InputHandler(ILogger<InputHandler> logger, IVersionManagerService versionManager, IFileSystem fileSystem)
    {
        _logger = logger;
        _versionManagerService = versionManager;
        _fileSystem = fileSystem;
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
                case Exit:
                    _logger.LogInformation(Exiting);
                    return;
                case Bugfix:
                    await _versionManagerService.UpdateVersionForBugfixRelease();
                    break;
                case Feature:
                    await _versionManagerService.UpdateVersionForFeatureRelease();
                    break;
            }
        }
    }

    private void SetFilePath()
    {
        while (true)
        {
            _logger.LogInformation(ProvidePath);
            var input = Console.ReadLine()?.Trim();

            if (_fileSystem.File.Exists(input))
            {
                ProductInfoFileConfigProvider.SwapInstance(new ProductInfoFileConfig(input));
                return;
            }
            
            _logger.LogInformation("File does not exist.");
        }
    }
    
    private void ProcessFilePathArgs(List<string> argsList)
    {
        var path = ExtractValueFromArgs(argsList, PathFlag);
        if (!_fileSystem.File.Exists(path))
        {
            _logger.LogError(FileNotFound, path);
            throw new ArgumentException();
        }
            
        ProductInfoFileConfigProvider.SwapInstance(new ProductInfoFileConfig(path));
    }

    private async Task ProcessReleaseTypeArgs(List<string> argsList)
    {
        var releaseType = ExtractValueFromArgs(argsList, ReleaseTypeFlag);
        var releaseTypes = new List<string> {Bugfix, Feature};
        if (!releaseTypes.Contains(releaseType))
        {
            _logger.LogError(InvalidReleaseTypeValue, releaseType);
            throw new ArgumentException();
        }

        if (releaseType == Bugfix)
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

        _logger.LogError(UnexpectedArgumentsFormat);
        throw new ArgumentException();
    }
}