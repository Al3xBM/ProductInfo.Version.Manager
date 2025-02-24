namespace ProductInfo.Version.Manager.Handlers;

public interface IInputHandler
{
    Task RunAsync(string[] args);
}