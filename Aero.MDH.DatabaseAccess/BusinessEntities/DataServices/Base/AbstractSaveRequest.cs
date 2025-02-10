using Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;
using Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Company;
using Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Contract;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.DataServices.Base;

public abstract class AbstractSaveRequest<T> where T : AbstractBusinessEntity
{
    private readonly ICollection<T> _entities;
    protected readonly List<IBusinessEntitySaver<T>> Savers = new();
    private readonly MdhDbContext _dbContext;
    private readonly IGlobalIdSaver<T> _globalIdSaver;
    private readonly IIdMasterSaver<T> _companyIdSaver;

    protected AbstractSaveRequest(IGlobalIdSaver<T> globalIdSaver, IIdMasterSaver<T> companyIdSaver, ICollection<T> entities, MdhDbContext dbContext)
    {
        _entities = entities;
        _dbContext = dbContext;
        _companyIdSaver = companyIdSaver;
        _globalIdSaver = globalIdSaver;
    }

    public async Task<SaveResult<T>> SaveAsync(CancellationToken cancellationToken = default)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var businessEntitiesToCreate = _entities.Where(e => e.Id == 0).ToList();

            if (businessEntitiesToCreate.Any())
            {
                await _globalIdSaver.CreateGlobalIdAsync(businessEntitiesToCreate);
                await _companyIdSaver.CreateIdMasterAsync(businessEntitiesToCreate);
            }
        
            foreach (var saver in Savers)
            {
                await saver.SaveAsync(_entities.ToList(), _dbContext, cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            return new SaveResult<T>(true, savedEntities: _entities.ToList());
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}