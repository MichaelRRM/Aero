using Aero.Application.Workers.EFrontToMdhFeeder;
using Aero.Application.Workers.TennaxiaDataCollection;
using Aero.Base.Attributes;

namespace Aero.Application.Workers;

[Injectable]
public class WorkerFactory
{
    private readonly Dictionary<string, IWorker> _mdhWorkerCodeToWorker = new();

    public WorkerFactory(TennaxiaDataCollectionWorker tennaxiaDataCollectionWorker, EFrontToMdhFeederWorker eFrontToMdhFeederWorker)
    {
        _mdhWorkerCodeToWorker.Add(tennaxiaDataCollectionWorker.Code, tennaxiaDataCollectionWorker);        
        _mdhWorkerCodeToWorker.Add(eFrontToMdhFeederWorker.Code, eFrontToMdhFeederWorker);
    }

    public IWorker GetWorker(string applicationName, string workerName)
    {
        switch (applicationName)
        {
            case "MDH":
                if (!_mdhWorkerCodeToWorker.TryGetValue(workerName, out var worker))
                {
                    throw new ArgumentException($"Unknown worker {workerName} in domain {applicationName}");
                }
                return worker;
        }
        throw new ArgumentException($"Unknown application name {applicationName}");
    }

    public IEnumerable<IWorker> GetWorkers()
    {
        return _mdhWorkerCodeToWorker.Values;
    }
}