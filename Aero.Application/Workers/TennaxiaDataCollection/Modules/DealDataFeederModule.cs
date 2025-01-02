namespace Aero.Application.Workers.TennaxiaDataCollection.Modules;

public class DealDataFeederModule : IModule
{
    public string Name => "DealDataFeeder";

    public Task RunAsync()
    {
        throw new Exception("test");
    }
}