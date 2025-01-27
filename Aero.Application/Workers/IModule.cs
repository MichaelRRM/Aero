namespace Aero.Application.Workers;

public interface IModule
{
    string Code { get; }
    string Name { get; }
    string Description { get; }
    Task RunAsync();
}