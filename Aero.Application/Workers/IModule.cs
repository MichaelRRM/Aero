namespace Aero.Application.Workers;

public interface IModule
{
    string Name { get; }
    string Description { get; }
    Task RunAsync();
}