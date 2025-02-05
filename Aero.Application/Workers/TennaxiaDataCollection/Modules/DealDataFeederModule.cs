using Microsoft.Extensions.Configuration;
using Aero.Tennaxia.ApiAccess;

namespace Aero.Application.Workers.TennaxiaDataCollection.Modules;

public class DealDataFeederModule : AbstractModule
{
    public override string Code => "DealDataFeeder";
    public override string Name => "Deal Data Feeder";
    public override string Description => "Imports data points for deals from Tennaxia into MDH";

    public StringArgument CampaignId => new(
        name: "CampaignId",
        isRequired: true,
        description: "Tennaxia ID of the campaign to import."
    );

    private readonly TennaxiaApiClient _tennaxiaApiClient;
    private readonly IConfiguration _configuration;

    public DealDataFeederModule(TennaxiaApiClient tennaxiaApiClient, IConfiguration configuration)
    {
        _tennaxiaApiClient = tennaxiaApiClient;
        _configuration = configuration;
    }

    public override async Task RunAsync()
    {
        var campaignId = CampaignId.GetValue(_configuration);
        
        var tennaxiaData = await _tennaxiaApiClient.GetAsync();
        throw new Exception("test");
    }
}