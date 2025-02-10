using Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.DataServices.Base;

public interface IBusinessEntitySaver<T> where T : AbstractBusinessEntity
{
    Task<SaveResult<T>> SaveAsync(List<T> entities, MdhDbContext dbContext, CancellationToken cancellationToken);
}