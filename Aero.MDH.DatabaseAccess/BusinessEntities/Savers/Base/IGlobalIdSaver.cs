using Aero.MDH.DatabaseAccess.BusinessEntities.Base;
using Aero.MDH.DatabaseAccess.BusinessEntities.DataServices.Base;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Base;

public interface IGlobalIdSaver<TBusinessEntity> where TBusinessEntity : AbstractBusinessEntity
{
    Task<SaveResult<TBusinessEntity>> CreateGlobalIdAsync(List<TBusinessEntity> businessEntitiesToCreate);
}