using Aero.MDH.DatabaseAccess.BusinessEntities.Models;
using Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Contract;
using Aero.MDH.DatabaseAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Company;

public class CompanyDataSaver : DataModelSaver<CompanyBusinessEntity, CompanyDatum>, ICompanyDataSaver
{
    public CompanyDataSaver(MdhDbContext dbContext) : base(dbContext)
    {
    }

    protected override int GetId(CompanyDatum databaseModel) => databaseModel.CompanyId;

    protected override async Task<IList<CompanyDatum>> GetExistingDatabaseModelsAsync(List<PreparationModel> preparationModels)
    {
        var ids = preparationModels.Select(p => p.InternalModel.Id).ToHashSet();
        return await GetDbSet().AsNoTracking().Where(d => ids.Contains(d.CompanyId)).ToListAsync();
    }

    protected override DbSet<CompanyDatum> GetDbSet()
    {
        return DbContext.CompanyData;
    }
}