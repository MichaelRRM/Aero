using Aero.MDH.DatabaseAccess.BusinessEntities.Models;
using Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Contract;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Company;

public class CompanyCodificationSaver : CodificationModelSaver<CompanyBusinessEntity>, ICompanyCodificationSaver
{
    public CompanyCodificationSaver(MdhDbContext dbContext) : base(dbContext)
    {
    }

    protected override IEnumerable<Codification> ConvertToDatabaseModels(CompanyBusinessEntity businessEntity)
    {
        aaa
    }
}