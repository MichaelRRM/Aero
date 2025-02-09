using Aero.Base.Extensions;
using Aero.MDH.DatabaseAccess.BusinessEntities.Base;
using Aero.MDH.DatabaseAccess.DataServices.Base;
using Microsoft.EntityFrameworkCore;

namespace Aero.MDH.DatabaseAccess.Savers;

public abstract class DatedModelSaver<TBusinessEntity, TDatabaseModel> : IBusinessEntitySaver<TBusinessEntity>
    where TBusinessEntity : AbstractBusinessEntity, new()
    where TDatabaseModel : class, IDatedTable, new()
{
    protected readonly MdhDbContext DbContext;
    
    protected DatedModelSaver(MdhDbContext dbContext)
    {
        DbContext = dbContext;
    }
    
    public async Task<SaveResult<TBusinessEntity>> SaveAsync(List<TBusinessEntity> entities, MdhDbContext dbContext,
        CancellationToken cancellationToken)
    {
        return await IntegrateAsync(entities, compareToExistingData: false);
    }

    private async Task<SaveResult<TBusinessEntity>> IntegrateAsync(List<TBusinessEntity> businessEntities, bool compareToExistingData)
    {
        var modelIdToExistingData = new Dictionary<int, List<TDatabaseModel>>();

        var preparationModels = businessEntities.Select(businessEntity => new PreparationModel(businessEntity, ConvertToDatabaseModels(businessEntity).ToList())).ToList();

        if (compareToExistingData)
        {
            var initialData = await GetExistingDatabaseModelsAsync(preparationModels);
            modelIdToExistingData = initialData.ToDictionaryList(i => i.Id);
        }

        var newData = new List<TDatabaseModel>();
        var changedData = new List<TDatabaseModel>();
        var unchangedData = new List<TDatabaseModel>();

        var changedModels = new List<TBusinessEntity>();

        foreach (var preparationModel in preparationModels)
        {
            if (!modelIdToExistingData.TryGetValue(preparationModel.InternalModel.Id, out var existingDatabaseModels))
            {
                existingDatabaseModels = new List<TDatabaseModel>();
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
            GetDbSet().AddRange(newData);
        }

        if (changedData.Any())
        {
            GetDbSet().UpdateRange(changedData);
        }

        if (unchangedData.Any())
        {
            HandleUnchangedData(unchangedData);
        }

        return new SaveResult<TBusinessEntity>(true, businessEntities);
    }

    protected virtual async Task<IList<TDatabaseModel>> GetExistingDatabaseModelsAsync(List<PreparationModel> preparationModels)
    {
        var ids = preparationModels.Select(p => p.InternalModel.Id).ToHashSet();
        return await GetDbSet().AsNoTracking().Where(d => ids.Contains(d.Id)).ToListAsync();
    }

    protected abstract DbSet<TDatabaseModel> GetDbSet();
    protected abstract IEnumerable<TDatabaseModel> ConvertToDatabaseModels(TBusinessEntity businessEntity);

    protected virtual void HandleUnchangedData(List<TDatabaseModel> unchangedData)
    {
    }

    /// <summary>
    /// Compares the newModels to the existingModels and splits them into new data, changed data and unchanged data. 
    /// </summary>
    /// <param name="newModels"></param>
    /// <param name="existingModels"></param>
    /// <returns></returns>
    protected virtual PreparationResult<TDatabaseModel> PrepareIntegration(List<TDatabaseModel> newModels, List<TDatabaseModel> existingModels)
    {
        var newData = new List<TDatabaseModel>();
        var changedData = new List<TDatabaseModel>();
        var unchangedData = new List<TDatabaseModel>();

        var preparationResult = new PreparationResult<TDatabaseModel>(newData, changedData, unchangedData);

        if (!newModels.Any())
        {
            return preparationResult;
        }

        var keyToSortedExistingModels = existingModels.GroupBy(GetKeyWithoutValueDate)
            .ToDictionary(elem => elem.Key,
                elem => new SortedList<DateOnly, TDatabaseModel>(elem.ToDictionary(e => e.ValueDate)));

        foreach (var newField in newModels)
        {
            var lastExistingValue = GetLastExistingValue(newField, keyToSortedExistingModels);
            if (lastExistingValue != null)
            {
                if (ValuesAreEqual(newField, lastExistingValue))
                {
                    DoWhenValuesAreEqual(newField, lastExistingValue, changedData, unchangedData);
                    continue;
                }

                if (newField.ValueDate == lastExistingValue.ValueDate)
                {
                    DoWhenValueHasChangedButValueDateIsTheSame(newField, lastExistingValue, changedData);
                    continue;
                }

                DoWhenValueHasChanged(newField, newData, keyToSortedExistingModels);
                continue;
            }

            DoWhenNoPreviousDataExists(newField, newData, keyToSortedExistingModels);
        }

        return preparationResult;
    }

    protected virtual void DoWhenNoPreviousDataExists(TDatabaseModel newField, List<TDatabaseModel> newData, Dictionary<object, SortedList<DateOnly, TDatabaseModel>> keyToSortedExistingModels)
    {
        if (!IsFieldValueNull(newField))
        {
            newData.Add(newField);

            var keyWithoutValueDate = GetKeyWithoutValueDate(newField);
            if (keyToSortedExistingModels.ContainsKey(keyWithoutValueDate))
            {
                keyToSortedExistingModels[keyWithoutValueDate].Add(newField.ValueDate, newField);
            }
            else
            {
                keyToSortedExistingModels[keyWithoutValueDate] =
                    new SortedList<DateOnly, TDatabaseModel>(
                        new List<TDatabaseModel>() { newField }.ToDictionary(n => n.ValueDate));
            }
        }
    }

    protected virtual void DoWhenValueHasChanged(TDatabaseModel newField, List<TDatabaseModel> newData, Dictionary<object, SortedList<DateOnly, TDatabaseModel>> keyToSortedExistingModels)
    {
        newData.Add(newField);
        keyToSortedExistingModels[GetKeyWithoutValueDate(newField)].Add(newField.ValueDate, newField);
    }

    protected virtual void DoWhenValueHasChangedButValueDateIsTheSame(TDatabaseModel newField,
        TDatabaseModel lastExistingValue, List<TDatabaseModel> changedData)
    {
        changedData.Add(newField);
    }

    protected virtual void DoWhenValuesAreEqual(TDatabaseModel newField, TDatabaseModel lastExistingValue,
        List<TDatabaseModel> changedData, List<TDatabaseModel> unchangedData)
    {
        unchangedData.Add(lastExistingValue);
    }

    /// <summary>
    /// Is the value considered null, and therefore should not be integrated unless a previous value exists. 
    /// </summary>
    /// <param name="newField"></param>
    /// <returns></returns>
    protected abstract bool IsFieldValueNull(TDatabaseModel newField);

    /// <summary>
    /// For a new field and an existing field with the same key, are the values equal.
    /// This is called after the keys have been matched, therefore there is no need to include the key in the comparison. 
    /// </summary>
    /// <param name="newField"></param>
    /// <param name="existingField"></param>
    /// <returns></returns>
    protected abstract bool ValuesAreEqual(TDatabaseModel newField, TDatabaseModel existingField);
    
    /// <summary>
    /// Returns the key which will identify database entries which should be compared based on their values  
    /// </summary>
    /// <param name="databaseModel"></param>
    /// <returns></returns>
    protected abstract object GetKeyWithoutValueDate(TDatabaseModel databaseModel);

    private TDatabaseModel? GetLastExistingValue(TDatabaseModel newValue,
        IDictionary<object, SortedList<DateOnly, TDatabaseModel>> keyToExistingValues)
    {
        if (keyToExistingValues.TryGetValue(GetKeyWithoutValueDate(newValue), out var existingValues))
        {
            return existingValues.LastOrDefault(a => a.Key <= newValue.ValueDate).Value;
        }

        return null;
    }

    protected record PreparationModel(TBusinessEntity InternalModel, List<TDatabaseModel> DatabaseModels);

    protected record PreparationResult<T>(List<T> NewData, List<T> ChangedData, List<T> UnchangedData);
}