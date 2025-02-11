using System.Linq.Expressions;
using System.Reflection;
using Aero.Base.Extensions;
using Aero.MDH.DatabaseAccess.BusinessEntities.DataServices.Base;
using Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;
using Aero.MDH.DatabaseAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Contract;

public abstract class CodificationModelSaver<TBusinessEntity, TCodificationModel> : IBusinessEntitySaver<TBusinessEntity> 
    where TBusinessEntity : AbstractBusinessEntity, IHasCodification<TCodificationModel>, new() 
    where TCodificationModel : new()
{
    private readonly MdhDbContext _dbContext;
    private static readonly Lazy<Func<TBusinessEntity, IEnumerable<Codification>>>
        LazyGetDatabaseModelsFromInternalModelFunc = new(BuildGetDatabaseModelsFromInternalModelFunc<TBusinessEntity>);

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

    private IEnumerable<Codification> ConvertToDatabaseModels(TBusinessEntity businessEntity)
    {
        var databaseModels = new List<Codification>();
        Feed(databaseModels, businessEntity);

        return databaseModels;
    }
    
    private static Func<TBusinessEntityType, IEnumerable<Codification>> BuildGetDatabaseModelsFromInternalModelFunc<TBusinessEntityType>()
        where TBusinessEntityType : AbstractBusinessEntity, IHasCodification<TCodificationModel>, new()
    {
        ParameterExpression internalModelParameterExpression = Expression.Parameter(typeof(TBusinessEntityType), "businessEntity");
        
        var allDatabaseModelCreationCallExpressions = typeof(TCodificationModel)
            .GetCodificationFieldProperties()
            .Select(
                x =>
                {
                    var codificationModelExpression = Expression.Property(internalModelParameterExpression, nameof(IHasCodification<TCodificationModel>.Codifications));
                    
                    var codificationFieldPropertyExpression =
                        Expression.Property(codificationModelExpression, x.PropertyInfo.Name);

                    var method = typeof(CodificationModelSaver<TBusinessEntity, TCodificationModel>).GetMethod(
                        nameof(GetDataBaseModelsFromCodificationField),
                        BindingFlags.NonPublic | BindingFlags.Static) ?? throw new NullReferenceException($"Couldn't create method for type {typeof(TCodificationModel).Name}");
                    
                    var databaseModelsCreationCallExpression = Expression.Call(
                        method.MakeGenericMethod(x.GenericArgument),
                        internalModelParameterExpression, 
                        codificationFieldPropertyExpression
                    );

                    return databaseModelsCreationCallExpression;
                }
            );

        var method = typeof(Enumerable).GetMethod(nameof(Enumerable.Concat), BindingFlags.Public | BindingFlags.Static) ?? throw new NullReferenceException($"Couldn't create method for type {typeof(TCodificationModel).Name}");
        
        var concatMethodInfo = method
            .MakeGenericMethod(typeof(Codification));

        var aggregatedCallExpression =
            allDatabaseModelCreationCallExpressions.Aggregate((x, y) => Expression.Call(null, concatMethodInfo, x, y));

        var lambdaExpression =
            Expression.Lambda<Func<TBusinessEntityType, IEnumerable<Codification>>>(aggregatedCallExpression,
                internalModelParameterExpression);

        return lambdaExpression.Compile();
    }

    private static IEnumerable<Codification> GetDataBaseModelsFromCodificationField<TWrapped>(TBusinessEntity businessEntity, CodificationField<TWrapped> codificationField)
    {
        var databaseModel = new Codification
        {
            CodificationCode = codificationField.Code,
        };

        if (codificationField.Value != null)
        {
            SetMatchingDataModelProperty(codificationField, databaseModel);
        }

        databaseModel.GlobalId = businessEntity.Id;

        yield return databaseModel;
    }
    
    private static void SetMatchingDataModelProperty<TWrapped>(CodificationField<TWrapped> codificationField, Codification databaseModel)
    {
        switch (codificationField.Value)
        {
            case int intWrappedType:
            {
                databaseModel.CodeNum = intWrappedType;
                break;
            }
            case string stringWrappedType:
            {
                databaseModel.CodeTxt = stringWrappedType;
                break;
            }
            default:
                throw new NotImplementedException($"Type of {codificationField.Value} is not supported");
        }
    }
    
    private void Feed(List<Codification> databaseModels, TBusinessEntity businessEntity)
    {
        databaseModels.AddRange(LazyGetDatabaseModelsFromInternalModelFunc.Value(businessEntity));
    }
    
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