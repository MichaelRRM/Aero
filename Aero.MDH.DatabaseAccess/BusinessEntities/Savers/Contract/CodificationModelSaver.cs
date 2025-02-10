using Aero.Base.Extensions;
using Aero.MDH.DatabaseAccess.BusinessEntities.DataServices.Base;
using Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;
using Microsoft.EntityFrameworkCore;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Contract;

public abstract class CodificationModelSaver<TBusinessEntity> : IBusinessEntitySaver<TBusinessEntity> where TBusinessEntity : AbstractBusinessEntity, new()
{
    private readonly MdhDbContext _dbContext;

    protected CodificationModelSaver(MdhDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SaveResult<TBusinessEntity>> SaveAsync(List<TBusinessEntity> entities, MdhDbContext dbContext,
        CancellationToken cancellationToken)
    {
        return await IntegrateAsync(entities);
    }

    private async Task<SaveResult<TBusinessEntity>> IntegrateAsync(List<TBusinessEntity> businessEntities)
    {
        var modelIdToExistingData = new Dictionary<int, List<Codification>>();

        var preparationModels = businessEntities.Select(businessEntity =>
            new PreparationModel(businessEntity, ConvertToDatabaseModels(businessEntity).ToList())).ToList();
        
        var initialData = await GetExistingDatabaseModelsAsync(preparationModels);
        modelIdToExistingData = initialData.ToDictionaryList(i => i.GlobalId);

        var newData = new List<Codification>();
        var changedData = new List<Codification>();
        var unchangedData = new List<Codification>();

        var changedModels = new List<TBusinessEntity>();

        foreach (var preparationModel in preparationModels)
        {
            if (!modelIdToExistingData.TryGetValue(preparationModel.InternalModel.Id, out var existingDatabaseModels))
            {
                existingDatabaseModels = new List<Codification>();
            }

            var preparationResult = PrepareIntegration(preparationModel.DatabaseModels, existingDatabaseModels);

            newData.AddRange(preparationResult.NewData);
            changedData.AddRange(preparationResult.ChangedData);
            unchangedData.AddRange(preparationResult.UnchangedData);

            if (preparationResult.NewData.Any() || preparationResult.ChangedData.Any())
            {
                changedModels.Add(preparationModel.InternalModel);
            }
        }

        if (newData.Any())
        {
            _dbContext.Codifications.AddRange(newData);
        }

        if (changedData.Any())
        {
            _dbContext.Codifications.UpdateRange(changedData);
        }
        
        return new SaveResult<TBusinessEntity>(true, businessEntities);
    }

    protected async Task<IList<Codification>> GetExistingDatabaseModelsAsync(
        List<PreparationModel> preparationModels)
    {
        var ids = preparationModels.Select(p => p.InternalModel.Id).ToHashSet();
        return await _dbContext.Codifications.AsNoTracking().Where(d => ids.Contains(d.GlobalId)).ToListAsync();
    }
    protected abstract IEnumerable<Codification> ConvertToDatabaseModels(TBusinessEntity businessEntity);
    
    /// <summary>
    /// Compares the newModels to the existingModels and splits them into new data, changed data and unchanged data. 
    /// </summary>
    /// <param name="newModels"></param>
    /// <param name="existingModels"></param>
    /// <returns></returns>
    protected virtual PreparationResult<Codification> PrepareIntegration(List<Codification> newModels,
        List<Codification> existingModels)
    {
        var newData = new List<Codification>();
        var changedData = new List<Codification>();
        var unchangedData = new List<Codification>();

        var preparationResult = new PreparationResult<Codification>(newData, changedData, unchangedData);

        if (!newModels.Any())
        {
            return preparationResult;
        }

        //TODO this sucks 
        var keyToSortedExistingModels = existingModels.GroupBy(GetKeyWithoutValueDate)
            .ToDictionary(elem => elem.Key,
                elem => new SortedList<int, Codification>(elem.ToDictionary(e => e.VersionId)));

        foreach (var newField in newModels)
        {
            var lastExistingValue = GetLastExistingValue(newField, keyToSortedExistingModels);
            if (lastExistingValue != null)
            {
                if (newField.CodeTxt == lastExistingValue.CodeTxt && newField.CodeNum == lastExistingValue.CodeNum)
                {
                    unchangedData.Add(lastExistingValue);
                    continue;
                }

                DoWhenValueHasChanged(newField, newData, keyToSortedExistingModels);
                continue;
            }

            DoWhenNoPreviousDataExists(newField, newData);
        }

        return preparationResult;
    }

    private void DoWhenNoPreviousDataExists(Codification newField, List<Codification> newData)
    {
        if (!(newField.CodeTxt == null && newField.CodeNum != null))
        {
            newData.Add(newField);
        }
    }

    private void DoWhenValueHasChanged(Codification newField, List<Codification> newData,
        Dictionary<object, SortedList<int, Codification>> keyToSortedExistingModels)
    {
        var lastVersion = keyToSortedExistingModels[GetKeyWithoutValueDate(newField)].Last().Value.VersionId;

        newField.VersionId = lastVersion + 1;
        newData.Add(newField);
    }
    
    /// <summary>
    /// Returns the key which will identify database entries which should be compared based on their values  
    /// </summary>
    /// <param name="databaseModel"></param>
    /// <returns></returns>
    private object GetKeyWithoutValueDate(Codification databaseModel)
    {
        return (databaseModel.GlobalId, databaseModel.CodificationCode);
    }

    private Codification? GetLastExistingValue(Codification newValue,
        IDictionary<object, SortedList<int, Codification>> keyToExistingValues)
    {
        if (keyToExistingValues.TryGetValue(GetKeyWithoutValueDate(newValue), out var existingValues))
        {
            return existingValues.LastOrDefault().Value;
        }

        return null;
    }

    protected record PreparationModel(TBusinessEntity InternalModel, List<Codification> DatabaseModels);

    protected record PreparationResult<T>(List<T> NewData, List<T> ChangedData, List<T> UnchangedData);
}