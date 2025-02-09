using Aero.MDH.DatabaseAccess.BusinessEntities;
using Aero.MDH.DatabaseAccess.DataServices.Base;

namespace Aero.MDH.DatabaseAccess.Savers.Company;

public class CompanyIdSaver : ICompanyIdSaver
{
    public Task<SaveResult<CompanyBusinessEntity>> SaveAsync(List<CompanyBusinessEntity> entities,
        MdhDbContext dbContext,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}