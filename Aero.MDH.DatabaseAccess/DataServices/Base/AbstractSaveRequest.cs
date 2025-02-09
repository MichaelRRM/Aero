using Aero.MDH.DatabaseAccess.BusinessEntities.Base;

namespace Aero.MDH.DatabaseAccess.DataServices.Base;

public abstract class AbstractSaveRequest<T> where T : AbstractBusinessEntity
{
    private readonly ICollection<T> _entities;
    protected readonly List<IBusinessEntitySaver<T>> Savers = new();
    private readonly MdhDbContext _dbContext;

    protected AbstractSaveRequest(ICollection<T> entities, MdhDbContext dbContext)
    {
        _entities = entities;
        _dbContext = dbContext;
    }

    public async Task<SaveResult<T>> SaveAsync(CancellationToken cancellationToken = default)
    {
        foreach (var saver in Savers)
        {
            await saver.SaveAsync(_entities.ToList(), _dbContext, cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return new SaveResult<T>(true, savedEntities: _entities.ToList());
    }
}