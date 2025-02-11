using Aero.MDH.DatabaseAccess.BusinessEntities.Models;
using Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Contract;
using Aero.MDH.DatabaseAccess.Models;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Company;

public class CompanyCodificationSaver : CodificationModelSaver<CompanyBusinessEntity, CompanyCodifications>, ICompanyCodificationSaver
{
    public CompanyCodificationSaver(MdhDbContext dbContext) : base(dbContext)
    {
    }
}