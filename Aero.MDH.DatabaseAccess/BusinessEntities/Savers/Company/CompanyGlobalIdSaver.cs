using Aero.MDH.DatabaseAccess.BusinessEntities.Models;
using Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Contract;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Company;

public class CompanyGlobalIdSaver : GlobalIdSaver<CompanyBusinessEntity>, ICompanyGlobalIdSaver
{
    protected override string BusinessEntityType => "company";

    public CompanyGlobalIdSaver(MdhDbContext mdhDbContext) : base(mdhDbContext)
    {
    }
}