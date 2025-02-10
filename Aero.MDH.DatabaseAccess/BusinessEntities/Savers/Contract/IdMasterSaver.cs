using Aero.MDH.DatabaseAccess.BusinessEntities.DataServices.Base;
using Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;
using Aero.MDH.DatabaseAccess.Models.Contract;
using Microsoft.EntityFrameworkCore;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Contract;

public abstract class IdMasterSaver<TBusinessEntity, TIdMaster> : IIdMasterSaver<TBusinessEntity>
    where TBusinessEntity : AbstractBusinessEntity where TIdMaster : class, IIdMaster
{
    private readonly MdhDbContext _mdhDbContext;

    protected IdMasterSaver(MdhDbContext mdhDbContext)
    {
        _mdhDbContext = mdhDbContext;
    }

    public async Task<SaveResult<TBusinessEntity>> CreateIdMasterAsync(List<TBusinessEntity> businessEntitiesToCreate)
    {
        var dbModels = businessEntitiesToCreate.Select(ConvertToDatabaseModel);

        await GetDbSet(_mdhDbContext).AddRangeAsync(dbModels);
        
        return new SaveResult<TBusinessEntity>(true, businessEntitiesToCreate);
    }

    protected abstract DbSet<TIdMaster> GetDbSet(MdhDbContext mdhDbContext);

    protected abstract TIdMaster ConvertToDatabaseModel(TBusinessEntity abstractBusinessEntity);
}