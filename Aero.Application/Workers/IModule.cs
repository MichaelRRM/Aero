namespace Aero.Application.Workers;

public interface IModule
{
    string Name { get; }
    Task RunAsync();
}