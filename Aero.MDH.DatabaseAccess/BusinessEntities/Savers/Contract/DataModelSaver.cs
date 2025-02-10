using System.Linq.Expressions;
using System.Reflection;
using Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;
using Aero.MDH.DatabaseAccess.Models.Contract;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Contract;

public abstract class DataModelSaver<TBusinessEntity, TDatabaseModel> : DatedModelSaver<TBusinessEntity, TDatabaseModel>
    where TBusinessEntity : AbstractBusinessEntity, new()
    where TDatabaseModel : class, IDataTable, new()
{
    private static readonly Lazy<Func<TBusinessEntity, IEnumerable<TDatabaseModel>>>
        LazyGetDatabaseModelsFromInternalModelFunc = new(BuildGetDatabaseModelsFromInternalModelFunc<TBusinessEntity>);

    protected DataModelSaver(MdhDbContext dbContext) : base(dbContext)
    {
    }

    protected override bool IsFieldValueNull(TDatabaseModel newField)
    {
        return newField.DataValueDate == null
               && newField.DataValueTxt == null
               && newField.DataValueBit == null
               && newField.DataValueInt == null;
    }

    protected override bool ValuesAreEqual(TDatabaseModel newField, TDatabaseModel existingField)
    {
        return newField.DataValueDate == existingField.DataValueDate
               && newField.DataValueInt == existingField.DataValueInt
               && newField.DataValueBit == existingField.DataValueBit
               && newField.DataValueInt == existingField.DataValueInt;
    }

    protected override object GetKeyWithoutValueDate(TDatabaseModel databaseModel)
    {
        return (databaseModel.Id, databaseModel.DataCode);
    }

    protected override IEnumerable<TDatabaseModel> ConvertToDatabaseModels(TBusinessEntity businessEntity)
    {
        var databaseModels = new List<TDatabaseModel>();
        Feed(databaseModels, businessEntity);

        return databaseModels;
    }

    private void Feed(List<TDatabaseModel> databaseModels, TBusinessEntity businessEntity)
    {
        databaseModels.AddRange(LazyGetDatabaseModelsFromInternalModelFunc.Value(businessEntity));
    }

    private static Func<TModel, IEnumerable<TDatabaseModel>> BuildGetDatabaseModelsFromInternalModelFunc<TModel>()
        where TModel : TBusinessEntity, new()
    {
        ParameterExpression internalModelParameterExpression = Expression.Parameter(typeof(TModel), "model");

        var allDatabaseModelCreationCallExpressions = typeof(TModel)
            .GetDatedFieldProperties()
            .Select(
                x =>
                {
                    var timeShiftingFieldPropertyExpression =
                        Expression.Property(internalModelParameterExpression, x.PropertyInfo.Name);

                    var databaseModelsCreationCallExpression = Expression.Call(
                        typeof(DatedModelSaver<,>),
                        nameof(GetDataBaseModelsFromTimeShiftingField),
                        new[] { x.GenericArgument },
                        internalModelParameterExpression, timeShiftingFieldPropertyExpression);

                    return databaseModelsCreationCallExpression;
                }
            );

        var concatMethodInfo = typeof(Enumerable)
            .GetMethod(nameof(Enumerable.Concat), BindingFlags.Public | BindingFlags.Static)
            .MakeGenericMethod(typeof(TDatabaseModel));

        var aggregatedCallExpression =
            allDatabaseModelCreationCallExpressions.Aggregate((x, y) => Expression.Call(null, concatMethodInfo, x, y));

        var lambdaExpression =
            Expression.Lambda<Func<TModel, IEnumerable<TDatabaseModel>>>(aggregatedCallExpression,
                internalModelParameterExpression);

        return lambdaExpression.Compile();
    }

    private static TDatabaseModel GetDataBaseModelsFromTimeShiftingField<TWrapped>(TBusinessEntity businessEntity,
        DatedField<TWrapped> datedField)
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
    
    private static void SetMatchingDataModelProperty<TWrapped>(DatedField<TWrapped> datedField, TDatabaseModel databaseModel)
    {
        switch (datedField.Value)
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
                throw new NotImplementedException($"Type of {datedField.Value} is not supported");
        }
    }
}