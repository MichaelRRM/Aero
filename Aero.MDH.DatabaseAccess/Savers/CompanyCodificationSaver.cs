using Aero.MDH.DatabaseAccess.BusinessEntities;

namespace Aero.MDH.DatabaseAccess.Savers;

public class CompanyCodificationSaver : ICompanyCodificationSaver
{
    public Task SaveAsync(CompanyBusinessEntity entity, MdhDbContext dbContext, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}