using Microsoft.Extensions.DependencyInjection;

namespace Aero.Application.Workers;

public class WorkerFactory
{
    private readonly TennaxiaDataCollection _tennaxiaDataCollection;

    public WorkerFactory(TennaxiaDataCollection tennaxiaDataCollection)
    {
        _tennaxiaDataCollection = tennaxiaDataCollection;
    }

    public IWorker GetWorker(string applicationName, string workerName)
    {
        return applicationName switch
        {
            "MDH" when workerName == "TennaxiaDataCollection" => _tennaxiaDataCollection,
            _ => throw new ArgumentException($"Unknown worker {workerName} in domain {applicationName}")
        };
    }
}