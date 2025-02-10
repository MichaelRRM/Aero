using Aero.EFront.DataAccess;
using Aero.MDH.DatabaseAccess.BusinessEntities;
using Aero.MDH.DatabaseAccess.BusinessEntities.DataServices;
using Aero.MDH.DatabaseAccess.BusinessEntities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Aero.Application.Workers.EFrontToMdhFeeder.Modules;

public class CompanyFeeder : AbstractModule
{
    private readonly IEFrontCompanyDataService _ieFrontCompanyDataService;
    private readonly ILogger<CompanyFeeder> _logger;
    private readonly IConfiguration _configuration;
    private readonly ICompanyDataService _companyDataService;

    public CompanyFeeder(IEFrontCompanyDataService ieFrontCompanyDataService, ILogger<CompanyFeeder> logger, IConfiguration configuration, ICompanyDataService companyDataService)
    {
        _ieFrontCompanyDataService = ieFrontCompanyDataService;
        _logger = logger;
        _configuration = configuration;
        _companyDataService = companyDataService;
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
    
    public override async Task RunAsync()
    {
        var processDate = ProcessDate.GetValue(_configuration);
        _logger.LogInformation($"Running company Feeder with process date {processDate:yyyy-MM-dd}");
        var companyData = _ieFrontCompanyDataService.GetEFrontCompanies();
        
        // mapping 

        var company1 = new CompanyBusinessEntity(){CompanyType = "testCompanyType"};
        company1.Name.Feed("test", DateOnly.FromDateTime(DateTime.Today));
        company1.Codifications.DealEFrontCode = "testCodif3";
        
        var company2 = new CompanyBusinessEntity(){ CompanyType = "testCompanyType"};
        company2.Name.Feed("test2", DateOnly.FromDateTime(DateTime.Today));
        
        var companies = new List<CompanyBusinessEntity>() { company1, company2 };
        
        // insert in db 
        var savedCompanies = await _companyDataService.CreateOrUpdate(companies)
            .WithCodifications()
            .WithData()
            .SaveAsync();
        
        return;
    }
}