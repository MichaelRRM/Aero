using Aero.Application.Workers.TennaxiaDataCollection;
using Microsoft.Extensions.DependencyInjection;

namespace Aero.Application.Workers;

public class WorkerFactory
{
    private readonly TennaxiaDataCollectionWorker _tennaxiaDataCollectionWorker;

    public WorkerFactory(TennaxiaDataCollectionWorker tennaxiaDataCollectionWorker)
    {
        _tennaxiaDataCollectionWorker = tennaxiaDataCollectionWorker;
    }

    public IWorker GetWorker(string applicationName, string workerName)
    {
        return applicationName switch
        {
            "MDH" when workerName == "TennaxiaDataCollection" => _tennaxiaDataCollectionWorker,
            _ => throw new ArgumentException($"Unknown worker {workerName} in domain {applicationName}")
        };
    }

    public IEnumerable<IWorker> GetWorkers()
    {
        yield return _tennaxiaDataCollectionWorker;
    }
}