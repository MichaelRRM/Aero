namespace Aero.Application.Workers;

public abstract class Worker : IWorker
{
    public abstract IEnumerable<IModule> Modules();

    public abstract string Name { get; }
    public abstract string Description { get; }

    public IModule GetModule(string moduleCode)
    {
        var modules = Modules();
        var module = modules.FirstOrDefault(m => m.Code == moduleCode);
        if (module == null)
        {
            throw new Exception($"Module {moduleCode} not found in {GetType().Name}");
        }
        return module;
    }
}