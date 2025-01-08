using Microsoft.Extensions.Configuration;
using Tennaxia.ApiAccess;

namespace Aero.Application.Workers.TennaxiaDataCollection.Modules;

public class DealDataFeederModule : IModule
{
    public string Name => "DealDataFeeder";

    public StringArgument CampaignId { get; } = new(
        name: "CampaignId",
        isRequired: true,
        description: "Campaign ID"
    );

    private readonly TennaxiaApiClient _tennaxiaApiClient;
    private readonly IConfiguration _configuration;

    public DealDataFeederModule(TennaxiaApiClient tennaxiaApiClient, IConfiguration configuration)
    {
        _tennaxiaApiClient = tennaxiaApiClient;
        _configuration = configuration;
    }

    public async Task RunAsync()
    {
        var campaignId = CampaignId.GetValue(_configuration);
        
        var tennaxiaData = await _tennaxiaApiClient.GetAsync();
        throw new Exception("test");
    }
}