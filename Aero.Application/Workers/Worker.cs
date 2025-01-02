namespace Aero.Application.Workers;

public abstract class Worker : IWorker
{
    protected abstract IEnumerable<IModule> GetModules();
    
    public IModule GetModule(string moduleName)
    {
        var modules = GetModules();
        var module = modules.FirstOrDefault(m => m.Name == moduleName);
        if (module == null)
        {
            throw new Exception($"Module {moduleName} not found in {GetType().Name}");
        }
        return module;
    }
}