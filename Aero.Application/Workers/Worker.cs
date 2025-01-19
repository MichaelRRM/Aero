namespace Aero.Application.Workers;

public abstract class Worker : IWorker
{
    public abstract IEnumerable<IModule> Modules();

    public abstract string Name { get; }
    public abstract string Description { get; }

    public IModule GetModule(string moduleName)
    {
        var modules = Modules();
        var module = modules.FirstOrDefault(m => m.Name == moduleName);
        if (module == null)
        {
            throw new Exception($"Module {moduleName} not found in {GetType().Name}");
        }
        return module;
    }
}