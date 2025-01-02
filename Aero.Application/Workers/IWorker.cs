namespace Aero.Application.Workers;

public interface IWorker
{
    IModule GetModule(string moduleName);
}