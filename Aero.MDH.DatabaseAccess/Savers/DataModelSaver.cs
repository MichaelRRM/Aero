using System.Linq.Expressions;
using System.Reflection;
using Aero.Base.Extensions;
using Aero.MDH.DatabaseAccess.BusinessEntities.Base;
using Aero.MDH.DatabaseAccess.DataServices.Base;

namespace Aero.MDH.DatabaseAccess.Savers;

public abstract class DataModelSaver<TBusinessEntity, TDatabaseModel> : IBusinessEntitySaver<TBusinessEntity>
    where TBusinessEntity : AbstractBusinessEntity, new()
    where TDatabaseModel : IDataTable, new()
{
    private readonly MdhDbContext _dbContext;

    private static readonly Lazy<Func<TBusinessEntity, IEnumerable<TDatabaseModel>>> LazyGetDatabaseModelsFromInternalModelFunc = new (BuildGetDatabaseModelsFromInternalModelFunc<TBusinessEntity>);

    protected DataModelSaver(MdhDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<SaveResult<TBusinessEntity>> SaveAsync(List<TBusinessEntity> entities, MdhDbContext dbContext,
        CancellationToken cancellationToken)
    {
        return await IntegrateAsync(entities, compareToExistingData: false);
    }

    private IEnumerable<TDatabaseModel> ConvertToDatabaseModels(AbstractBusinessEntity businessEntity)
    {
        var databaseModels = new List<TDatabaseModel>();
        Feed(databaseModels, businessEntity);

        return databaseModels;
    }

    protected virtual void Feed(List<TDatabaseModel> databaseModels, AbstractBusinessEntity businessEntity)
    {
        databaseModels.AddRange(LazyGetDatabaseModelsFromInternalModelFunc.Value(businessEntity));
    }
    
    private static Func<TModel, IEnumerable<TDatabaseModel>> BuildGetDatabaseModelsFromInternalModelFunc<TModel>() where TModel : AbstractBusinessEntity, new()
    {
        ParameterExpression internalModelParameterExpression = Expression.Parameter(typeof(TModel), "model");

        var allDatabaseModelCreationCallExpressions = typeof(TModel)
            .GetTimeShiftingProperties()
            .Select(
                x =>
                {
                    var timeShiftingFieldPropertyExpression = Expression.Property(internalModelParameterExpression, x.PropertyInfo.Name);

                    var databaseModelsCreationCallExpression = Expression.Call(
                        typeof(DataModelSaver<TModel, TDatabaseModel, TRefModel>),
                        nameof(GetDataBaseModelsFromTimeShiftingField),
                        new[] { x.GenericArgument },
                        internalModelParameterExpression, timeShiftingFieldPropertyExpression);

                    return databaseModelsCreationCallExpression;
                }
            );
        
        var concatMethodInfo = typeof(Enumerable).GetMethod(nameof(Enumerable.Concat), BindingFlags.Public | BindingFlags.Static)
            .MakeGenericMethod(typeof(TDatabaseModel));

        var aggregatedCallExpression = allDatabaseModelCreationCallExpressions.Aggregate((x, y) => Expression.Call(null, concatMethodInfo, x, y));

        var lambdaExpression = Expression.Lambda<Func<TModel, IEnumerable<TDatabaseModel>>>(aggregatedCallExpression, internalModelParameterExpression);

        return lambdaExpression.Compile();
    }
    
    private static TDatabaseModel GetDataBaseModelsFromTimeShiftingField<TWrapped>(TBusinessEntity businessEntity, DatedField<TWrapped> datedField)
    {
        var databaseModel = new TDatabaseModel
        {
            DataCode = datedField.Code,
            ValueDate = datedField.ValueDate,
        };

        if (datedField.Value != null)
        {
            SetMatchingDataModelProperty(datedField, databaseModel);
        }

        databaseModel.Id = businessEntity.Id;

        return databaseModel;
    }
    
    private static void SetMatchingDataModelProperty<TWrappedType>(KeyValuePair<(DateTime, FieldDataSource), SourcedField<TWrappedType>> obj, TDatabaseModel databaseModel)
    {
        switch (obj.Value.Value)
        {
            case bool booleanWrappedType:
            {
                databaseModel.DataValueBit = booleanWrappedType;
                break;
            }
            case int intWrappedType:
            {
                databaseModel.DataValueInt = intWrappedType;
                break;
            }
            case string stringWrappedType:
            {
                databaseModel.DataValueTxt = stringWrappedType;
                break;
            }
            case DateOnly dateWrappedType:
            {
                databaseModel.DataValueDate = dateWrappedType;
                break;
            }
            default:
                throw new NotImplementedException($"Type of {obj.Value.Value} not managed yet");
        }
    }

    private async Task<SaveResult<TBusinessEntity>> IntegrateAsync(List<TBusinessEntity> businessEntities, bool compareToExistingData)
    {
        var modelIdToExistingData = new Dictionary<int, List<TDatabaseModel>>();

        var preparationModels = businessEntities.Select(m => new PreparationModel()
            { InternalModel = m, DatabaseModels = ConvertToDatabaseModels(m).ToList() }).ToList();

        if (compareToExistingData)
        {
            var initialData = GetExistingDatabaseModels(preparationModels);
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
            _dbContext.AddRange(newData);
        }

        if (changedData.Any())
        {
            // TODO need to do something here and make sure there is no tracking 
            aaa
        }

        if (unchangedData.Any())
        {
            HandleUnchangedData(unchangedData);
        }

        return new SaveResult<TBusinessEntity>(true, businessEntities);
    }

    protected abstract IList<TDatabaseModel> GetExistingDatabaseModels(List<PreparationModel> preparationModels);

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

        var preparationResult = new PreparationResult<TDatabaseModel>()
        {
            NewData = newData,
            ChangedData = changedData,
            UnchangedData = unchangedData
        };

        if (!newModels.Any())
        {
            return preparationResult;
        }

        var keyToSortedExistingModels = existingModels.GroupBy(GetKeyWithoutValueDate)
            .ToDictionary(elem => elem.Key,
                elem => new SortedList<DateTime, TDatabaseModel>(elem.ToDictionary(e => e.ValueDate)));

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

    protected virtual void DoWhenNoPreviousDataExists(TDatabaseModel newField, List<TDatabaseModel> newData, Dictionary<object, SortedList<DateTime, TDatabaseModel>> keyToSortedExistingModels)
    {
        if (!IsFieldValueNull(newField))
        {
            newData.Add(newField);

            var keyWithoutValueDate = GetKeyWithoutValueDate(newField);
            //Checking if keyToSortedExistingModels is empty as it may have data with a future value date, even if no data exists at an earlier value date
            if (keyToSortedExistingModels.ContainsKey(keyWithoutValueDate))
            {
                keyToSortedExistingModels[keyWithoutValueDate].Add(newField.ValueDate, newField);
            }
            else
            {
                keyToSortedExistingModels[keyWithoutValueDate] =
                    new SortedList<DateTime, TDatabaseModel>(
                        new List<TDatabaseModel>() { newField }.ToDictionary(n => n.ValueDate));
            }
        }
    }

    protected virtual void DoWhenValueHasChanged(TDatabaseModel newField, List<TDatabaseModel> newData, Dictionary<object, SortedList<DateTime, TDatabaseModel>> keyToSortedExistingModels)
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
        IDictionary<object, SortedList<DateTime, TDatabaseModel>> keyToExistingValues)
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