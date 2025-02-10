using Aero.MDH.DatabaseAccess.BusinessEntities.DataServices.Base;
using Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Contract;

public interface IIdMasterSaver<TBusinessEntity> where TBusinessEntity : AbstractBusinessEntity
{
    Task<SaveResult<TBusinessEntity>> CreateIdMasterAsync(List<TBusinessEntity> businessEntitiesToCreate);
}