using Aero.MDH.DatabaseAccess.BusinessEntities;
using Aero.MDH.DatabaseAccess.DataServices.Base;
using Aero.MDH.DatabaseAccess.Savers.Company;

namespace Aero.MDH.DatabaseAccess.DataServices;

public class CompanySaveRequest : AbstractSaveRequest<CompanyBusinessEntity>
{
    private readonly ICompanyGlobalIdSaver _companyGlobalIdSaver;
    private readonly ICompanyIdSaver _companyIdSaver;
    private readonly ICompanyCodificationSaver _companyCodificationSaver;
    private readonly ICompanyDataSaver _companyDataSaver;
    
    public CompanySaveRequest(ICollection<CompanyBusinessEntity> entities, MdhDbContext dbContext, ICompanyGlobalIdSaver companyGlobalIdSaver, ICompanyIdSaver companyIdSaver, ICompanyCodificationSaver companyCodificationSaver, ICompanyDataSaver companyDataSaver) : base(entities, dbContext)
    {
        _companyGlobalIdSaver = companyGlobalIdSaver;
        _companyIdSaver = companyIdSaver;
        _companyCodificationSaver = companyCodificationSaver;
        _companyDataSaver = companyDataSaver;
    }

    public CompanySaveRequest WithData()
    {
        Savers.Add(_companyDataSaver);
        return this;
    }

    public CompanySaveRequest WithCodifications()
    {
        Savers.Add(_companyCodificationSaver);
        return this;
    }
}