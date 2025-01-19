using Aero.Application.Workers.TennaxiaDataCollection.Modules;

namespace Aero.Application.Workers.TennaxiaDataCollection;

public class TennaxiaDataCollectionWorker : Worker
{
    private readonly DealDataFeederModule _dealDataFeederModule;
    private readonly TestModule _testModule;

    public override string Name => "Tennaxia Data Collection";
    public override string Description => "These modules import data from Tennaxia into MDH";

    public override IEnumerable<IModule> Modules()
    {
        yield return _dealDataFeederModule;
        yield return _testModule;
    }
    
    public TennaxiaDataCollectionWorker(DealDataFeederModule dealDataFeederModule, TestModule testModule)
    {
        _dealDataFeederModule = dealDataFeederModule;
        _testModule = testModule;
    }
}