using Aero.MDH.DatabaseAccess.BusinessEntities.Models;
using Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Contract;
using Aero.MDH.DatabaseAccess.Models;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Company;

public class CompanyCodificationSaver : CodificationModelSaver<CompanyBusinessEntity>, ICompanyCodificationSaver
{
    public CompanyCodificationSaver(MdhDbContext dbContext) : base(dbContext)
    {
    }

    protected override IEnumerable<Codification> ConvertToDatabaseModels(CompanyBusinessEntity businessEntity)
    {
        if (businessEntity.Codifications.DealEFrontCode != null)
        {
            yield return new Codification
            {
                GlobalId = businessEntity.Id,
                CodificationCode = "DEAL_EFRONT_CODE",
                VersionId = 0,
                CodeTxt = businessEntity.Codifications.DealEFrontCode,
            };
        }
    }
}