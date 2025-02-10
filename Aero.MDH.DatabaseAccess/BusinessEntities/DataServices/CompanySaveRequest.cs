using Aero.MDH.DatabaseAccess.BusinessEntities.DataServices.Base;
using Aero.MDH.DatabaseAccess.BusinessEntities.Models;
using Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Company;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.DataServices;

public class CompanySaveRequest : AbstractSaveRequest<CompanyBusinessEntity>
{
    private readonly ICompanyCodificationSaver _companyCodificationSaver;
    private readonly ICompanyDataSaver _companyDataSaver;
    
    public CompanySaveRequest(ICompanyGlobalIdSaver companyGlobalIdSaver, ICompanyIdSaver companyIdSaver,
        ICollection<CompanyBusinessEntity> entities,
        MdhDbContext dbContext, ICompanyCodificationSaver companyCodificationSaver,
        ICompanyDataSaver companyDataSaver) : base(companyGlobalIdSaver, companyIdSaver, entities, dbContext)
    {
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