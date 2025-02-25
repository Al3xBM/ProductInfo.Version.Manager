using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using static ProductInfo.Version.Manager.Common.Constants;
using static ProductInfo.Version.Manager.Handlers.InputHandler;
using static ProductInfo.Version.Manager.Tests.Common.Constants;

namespace ProductInfo.Version.Manager.Tests.InputHandlerTests;

public class RunAsyncShould : InputHandlerTestBase
{
    public static IEnumerable<object[]> BadArgsProvider =>
        new List<object[]>
        {
            new object[] { new[] { PathFlag, DefaultPath, ReleaseTypeFlag }, UnexpectedArgumentsFormat },
            new object[] { new[] { PathFlag, DefaultPath }, UnexpectedArgumentsFormat },
            new object[] { new[] { PathFlag }, UnexpectedArgumentsFormat }

        };
    
    private readonly Func<string[], Task> _act;

    public RunAsyncShould()
    {
        _act = Service.RunAsync;
    }

    [Theory(Skip = "Skipping this for now")]
    [InlineData(Feature)]
    [InlineData(Bugfix)]
    public async Task ProcessInputArgs_When_Present(string releaseType)
    {
        var act = async () => await _act([PathFlag, DefaultPath, ReleaseTypeFlag, releaseType]);

        await act.Should().NotThrowAsync();
    }

    [Theory]
    [MemberData(nameof(BadArgsProvider))]
    public async Task ThrowArgumentException_When_DidNotReceiveArgumentsInExpectedFormat(string[] args, string logMessage)
    {
        var act = async () => await _act(args);
        
        await act.Should().ThrowExactlyAsync<ArgumentException>();
        CheckLogCall(logMessage, LogLevel.Error);
    }

    [Fact]
    public async Task ThrowArgumentException_When_ArgsSpecifiedFilePathNotFound()
    {
        FileSystem.RemoveFile(DefaultPath);
        
        var act = async () => await _act([PathFlag, DefaultPath, ReleaseTypeFlag, Feature]);

        await act.Should().ThrowExactlyAsync<ArgumentException>();
        CheckLogCall(string.Format(FileNotFound, DefaultPath), LogLevel.Error);
    }
    
    [Fact]
    public async Task ThrowArgumentException_When_ReleaseTypeIsInvalid()
    {
        var dummyValue = "dummy";
        
        var act = async () => await _act([PathFlag, DefaultPath, ReleaseTypeFlag, dummyValue]);

        await act.Should().ThrowExactlyAsync<ArgumentException>();
        CheckLogCall(string.Format(InvalidReleaseTypeValue, dummyValue), LogLevel.Error);
    }

    [Fact]
    public async Task WaitForUserInput_When_ArgsNotPresent()
    {
        var stringReader = new StringReader($"{DefaultPath}\nexit");
        Console.SetIn(stringReader);
        
        var act = async () => await _act([]);
        
        await act.Should().NotThrowAsync();
        CheckLogCall(ProvidePath, LogLevel.Information);
        CheckLogCall(Exiting, LogLevel.Information);
    }

    [Fact]
    public async Task UpdateMinorVersion_When_UserInputIsBugfix()
    {
        var stringReader = new StringReader($"{DefaultPath}\n{Bugfix}\nexit");
        Console.SetIn(stringReader);

        var act = async () => await _act([]);
        
        await act.Should().NotThrowAsync();
        CheckLogCall(ProvidePath, LogLevel.Information);
        CheckLogCall(Exiting, LogLevel.Information);
        var fileContent = await FileSystem.File.ReadAllTextAsync(DefaultPath);
        fileContent.Trim().Should().Be(BugfixIncrementedVersion);
    }
    
    [Fact]
    public async Task UpdateMajorVersion_When_UserInputIsFeature()
    {
        var stringReader = new StringReader($"{DefaultPath}\n{Feature}\nexit");
        Console.SetIn(stringReader);

        var act = async () => await _act([]);
        
        await act.Should().NotThrowAsync();
        CheckLogCall(ProvidePath, LogLevel.Information);
        CheckLogCall(Exiting, LogLevel.Information);
        var fileContent = await FileSystem.File.ReadAllTextAsync(DefaultPath);
        fileContent.Trim().Should().Be(FeatureIncrementedVersion);
    }
    
    private void CheckLogCall(string message, LogLevel logLevel)
    {
        Logger
            .Received(1)
            .Log(
                logLevel,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString() == message),
                null,
                Arg.Any<Func<object, Exception, string>>()!);
    }
}