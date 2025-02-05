using Aero.Application.Workers.EFrontToMdhFeeder.Modules;

namespace Aero.Application.Workers.EFrontToMdhFeeder;

public class EFrontToMdhFeederWorker : Worker
{
    private readonly CompanyFeeder _companyFeeder;

    public EFrontToMdhFeederWorker(CompanyFeeder companyFeeder)
    {
        _companyFeeder = companyFeeder;
    }

    public override IEnumerable<IModule> Modules()
    {
        yield return _companyFeeder;
    }

    public override string Code => "EFrontToMdh";
    public override string Name => "eFront To MDH Feeder";

    public override string Description => "This worker imports data from eFront to MDH";
}