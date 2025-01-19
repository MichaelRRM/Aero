using System.Reflection;
using Aero.Application.Workers;
using Aero.Worker.WebApi.Models;

namespace Aero.Worker.WebApi.Services;

public class WorkerService : IWorkerService
{
    private readonly WorkerFactory _workerFactory;

    public WorkerService(WorkerFactory workerFactory)
    {
        _workerFactory = workerFactory;
    }

    public IEnumerable<WorkerDto> GetWorkers()
    {
        var workers = _workerFactory.GetWorkers();
        return workers.Select(ToDto);
    }

    private WorkerDto ToDto(IWorker worker)
    {
        var modules = worker.Modules().Select(ToDto);
        return new WorkerDto(Name: worker.Name, Description: worker.Description, Modules: modules);
    }

    private ModuleDto ToDto(IModule module)
    {
        var arguments = GetArguments(module);
        return new ModuleDto(Name: module.Name, Description: module.Description, Arguments: arguments);
    }

    private IEnumerable<ArgumentDto> GetArguments(IModule module)
    {
        var stringArguments = module.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
            //TODO handle generic arguments 
            .Where(p => p.PropertyType.IsAssignableFrom(typeof(StringArgument)))
            .Select(p => p.GetValue(module))
            .Cast<StringArgument>();

        foreach (var stringArgument in stringArguments.Select(a => new ArgumentDto(
                     Name: a.Name,
                     Description: a.Description,
                     Type: "string",
                     DefaultValue: a.DefaultValue?.ToString(),
                     IsRequired: a.IsRequired)
                 ))
        {
            yield return stringArgument;
        }

        var dateArguments = module.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
            //TODO handle generic arguments 
            .Where(p => p.PropertyType.IsAssignableFrom(typeof(DateOnlyArgument)))
            .Select(p => p.GetValue(module))
            .Cast<DateOnlyArgument>();

        foreach (var stringArgument in dateArguments.Select(a => new ArgumentDto(
                     Name: a.Name,
                     Description: a.Description,
                     Type: "date",
                     DefaultValue: a.HasDefaultValue ? a.DefaultValue.ToString() : null,
                     IsRequired: a.IsRequired)
                 ))
        {
            yield return stringArgument;
        }
    }
}