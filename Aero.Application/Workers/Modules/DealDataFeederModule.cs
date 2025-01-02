namespace Aero.Application.Workers.Modules;

public class DealDataFeederModule : IModule
{
    public string Name => "DealDataFeeder";

    public Task RunAsync()
    {
        throw new Exception("test");
    }
}