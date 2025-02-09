using Aero.MDH.DatabaseAccess.BusinessEntities;

namespace Aero.MDH.DatabaseAccess.Savers;

public class CompanyIdSaver : ICompanyIdSaver
{
    public Task SaveAsync(CompanyBusinessEntity entity, MdhDbContext dbContext, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}