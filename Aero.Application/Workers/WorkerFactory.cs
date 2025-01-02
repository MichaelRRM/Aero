using Microsoft.Extensions.DependencyInjection;

namespace Aero.Application.Workers;

public static class WorkerFactory
{
    public static IWorker GetWorker(string applicationName, string workerName, IServiceProvider serviceProvider)
    {
        return applicationName switch
        {
            "MDH" when workerName == "TennaxiaDataCollection" => serviceProvider.GetRequiredService<TennaxiaDataCollection>(),
            _ => throw new ArgumentException($"Unknown worker {workerName} in domain {applicationName}")
        };
    }
}