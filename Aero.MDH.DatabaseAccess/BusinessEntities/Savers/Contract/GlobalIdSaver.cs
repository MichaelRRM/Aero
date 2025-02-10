using Aero.MDH.DatabaseAccess.BusinessEntities.DataServices.Base;
using Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Contract;

public abstract class GlobalIdSaver<TBusinessEntity> : IGlobalIdSaver<TBusinessEntity> where TBusinessEntity : AbstractBusinessEntity
{
    private readonly MdhDbContext _mdhDbContext;

    protected GlobalIdSaver(MdhDbContext mdhDbContext)
    {
        _mdhDbContext = mdhDbContext;
    }

    protected abstract string BusinessEntityType { get;}


    public async Task<SaveResult<TBusinessEntity>> CreateGlobalIdAsync(List<TBusinessEntity> businessEntitiesToCreate)
    {
        var businessModelsAndGlobalIds = new List<BusinessModelAndGlobalId>();

        foreach (var businessEntityToCreate in businessEntitiesToCreate)
        {
            var globalId = new GlobalIdMaster
            {
                ObjectType = BusinessEntityType,
            };
            businessModelsAndGlobalIds.Add(new BusinessModelAndGlobalId(businessEntityToCreate, globalId));
        }

        await _mdhDbContext.GlobalIdMasters.AddRangeAsync(businessModelsAndGlobalIds.Select(bm => bm.GlobalIdMaster));

        foreach (var businessModelsAndGlobalId in businessModelsAndGlobalIds)
        {
            businessModelsAndGlobalId.BusinessEntity.Id = businessModelsAndGlobalId.GlobalIdMaster.GlobalId;
        }

        return new SaveResult<TBusinessEntity>(true, businessEntitiesToCreate);
    }
    
    private record BusinessModelAndGlobalId(TBusinessEntity BusinessEntity, GlobalIdMaster GlobalIdMaster);
}