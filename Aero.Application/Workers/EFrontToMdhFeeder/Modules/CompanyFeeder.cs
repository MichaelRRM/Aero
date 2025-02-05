using Aero.EFront.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Aero.Application.Workers.EFrontToMdhFeeder.Modules;

public class CompanyFeeder : AbstractModule
{
    private readonly IEFrontCompanyDataService _ieFrontCompanyDataService;
    private readonly ILogger<CompanyFeeder> _logger;
    private readonly IConfiguration _configuration;

    public CompanyFeeder(IEFrontCompanyDataService ieFrontCompanyDataService, ILogger<CompanyFeeder> logger, IConfiguration configuration)
    {
        _ieFrontCompanyDataService = ieFrontCompanyDataService;
        _logger = logger;
        _configuration = configuration;
    }

    public override string Code => "CompanyFeeder";
    public override string Name => "Company Feeder";
    public override string Description => "Feeds company data";

    public DateOnlyArgument ProcessDate => new(
        name: "ProcessDate",
        isRequired: false,
        defaultValue: DateOnly.FromDateTime(DateTime.Today),
        description: "The value date used for requesting and inserting company data"
    );
    
    public override Task RunAsync()
    {
        var processDate = ProcessDate.GetValue(_configuration);
        _logger.LogInformation($"Running company Feeder with process date {processDate:yyyy-MM-dd}");
        var companyData = _ieFrontCompanyDataService.GetEFrontCompanies();
        
        // mapping 
        
        // insert in db 
        
        return Task.CompletedTask;
    }
}