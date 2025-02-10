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
        var businessEntityAndGlobalIds = new List<BusinessModelAndGlobalId>();

        foreach (var businessEntityToCreate in businessEntitiesToCreate)
        {
            var globalId = new GlobalIdMaster
            {
                ObjectType = BusinessEntityType,
            };
            businessEntityAndGlobalIds.Add(new BusinessModelAndGlobalId(businessEntityToCreate, globalId));
        }

        var dbModels = businessEntityAndGlobalIds.Select(bm => bm.GlobalIdMaster).ToList();

        await _mdhDbContext.GlobalIdMasters.AddRangeAsync(dbModels);
        await _mdhDbContext.SaveChangesAsync();

        //this ensures that business entities get the right id generated from the DB.
        AssignIdToBusinessEntity(businessEntityAndGlobalIds);

        return new SaveResult<TBusinessEntity>(true, businessEntitiesToCreate);
    }
    
    private static void AssignIdToBusinessEntity(List<BusinessModelAndGlobalId> businessModelsAndGlobalIds)
    {
        foreach (var businessModelsAndGlobalId in businessModelsAndGlobalIds)
        {
            businessModelsAndGlobalId.BusinessEntity.Id = businessModelsAndGlobalId.GlobalIdMaster.GlobalId;
        }
    }

    private record BusinessModelAndGlobalId(TBusinessEntity BusinessEntity, GlobalIdMaster GlobalIdMaster);
}