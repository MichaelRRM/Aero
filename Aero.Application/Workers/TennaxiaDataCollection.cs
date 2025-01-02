using Aero.Application.Workers.Modules;

namespace Aero.Application.Workers;

public class TennaxiaDataCollection : Worker
{
    private readonly DealDataFeederModule _dealDataFeederModule;

    protected override IEnumerable<IModule> GetModules()
    {
        yield return _dealDataFeederModule;
    }
    
    public TennaxiaDataCollection(DealDataFeederModule dealDataFeederModule)
    {
        _dealDataFeederModule = dealDataFeederModule;
    }
}