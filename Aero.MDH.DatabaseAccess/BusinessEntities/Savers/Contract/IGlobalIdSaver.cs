using Aero.MDH.DatabaseAccess.BusinessEntities.DataServices.Base;
using Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Contract;

public interface IGlobalIdSaver<TBusinessEntity> where TBusinessEntity : AbstractBusinessEntity
{
    Task<SaveResult<TBusinessEntity>> CreateGlobalIdAsync(List<TBusinessEntity> businessEntitiesToCreate);
}