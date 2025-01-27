namespace Aero.Application.Workers;

public interface IWorker
{
    string Name { get; }
    string Description { get; }
    IModule GetModule(string moduleCode);
    IEnumerable<IModule> Modules();
}