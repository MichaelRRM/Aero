using Tennaxia.ApiAccess;

namespace Aero.Application.Workers.TennaxiaDataCollection.Modules;

public class DealDataFeederModule : IModule
{
    public string Name => "DealDataFeeder";

    private readonly TennaxiaApiClient _tennaxiaApiClient;

    public DealDataFeederModule(TennaxiaApiClient tennaxiaApiClient)
    {
        _tennaxiaApiClient = tennaxiaApiClient;
    }

    public async Task RunAsync()
    {
        var tennaxiaData = await _tennaxiaApiClient.GetAsync();
        throw new Exception("test");
    }
}