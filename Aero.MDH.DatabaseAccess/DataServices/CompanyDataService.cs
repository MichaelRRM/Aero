using Aero.MDH.DatabaseAccess.BusinessEntities;
using Aero.MDH.DatabaseAccess.DataServices.Base;
using Aero.MDH.DatabaseAccess.Savers.Company;

namespace Aero.MDH.DatabaseAccess.DataServices;

public class CompanyDataService : ICompanyDataService
{
    private readonly MdhDbContext _dbContext;
    private readonly ICompanyGlobalIdSaver _companyGlobalIdSaver;
    private readonly ICompanyIdSaver _companyIdSaver;
    private readonly ICompanyCodificationSaver _companyCodificationSaver;
    private readonly ICompanyDataSaver _companyDataSaver;

    public CompanyDataService(MdhDbContext dbContext, ICompanyGlobalIdSaver companyGlobalIdSaver, ICompanyIdSaver companyIdSaver, ICompanyCodificationSaver companyCodificationSaver, ICompanyDataSaver companyDataSaver)
    {
        _dbContext = dbContext;
        _companyGlobalIdSaver = companyGlobalIdSaver;
        _companyIdSaver = companyIdSaver;
        _companyCodificationSaver = companyCodificationSaver;
        _companyDataSaver = companyDataSaver;
    }

    public CompanySaveRequest CreateOrUpdate(ICollection<CompanyBusinessEntity> companies)
    {
        return new CompanySaveRequest(companies, _dbContext, _companyGlobalIdSaver, _companyIdSaver, _companyCodificationSaver, _companyDataSaver)
            .WithData()
            .WithCodifications();
    }
}