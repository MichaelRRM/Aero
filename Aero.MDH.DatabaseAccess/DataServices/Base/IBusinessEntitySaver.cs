using Aero.MDH.DatabaseAccess.BusinessEntities.Base;

namespace Aero.MDH.DatabaseAccess.DataServices.Base;

public interface IBusinessEntitySaver<T> where T : AbstractBusinessEntity
{
    Task<SaveResult<T>> SaveAsync(List<T> entities, MdhDbContext dbContext, CancellationToken cancellationToken);
}