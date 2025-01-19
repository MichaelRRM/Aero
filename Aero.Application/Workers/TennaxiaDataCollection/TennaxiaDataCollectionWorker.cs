using Aero.Application.Workers.TennaxiaDataCollection.Modules;

namespace Aero.Application.Workers.TennaxiaDataCollection;

public class TennaxiaDataCollectionWorker : Worker
{
    private readonly DealDataFeederModule _dealDataFeederModule;

    public override string Name => "Tennaxia Data Collection";
    public override string Description => "These modules import data from Tennaxia into MDH";

    public override IEnumerable<IModule> Modules()
    {
        yield return _dealDataFeederModule;
    }
    
    public TennaxiaDataCollectionWorker(DealDataFeederModule dealDataFeederModule)
    {
        _dealDataFeederModule = dealDataFeederModule;
    }
}