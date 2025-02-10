using System.Collections.Concurrent;
using System.Reflection;
using Aero.Base.Extensions;
using Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;
using Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Contract;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Savers;

public static class TypeExtensions
{
    private static readonly ConcurrentDictionary<Type, List<DatedFieldProperty>> TypeToTimeShiftingProperty = new();
    
    public static List<DatedFieldProperty> GetDatedFieldProperties(this Type typeToSearch)
    {
        return TypeToTimeShiftingProperty.GetOrAdd(typeToSearch, HandleGetDatedFieldProperties);
    }

    private static List<DatedFieldProperty> HandleGetDatedFieldProperties(this Type typeToSearch)
    {
        var instance = Activator.CreateInstance(typeToSearch);
    
        return typeToSearch
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(x => (
                PropertyInfo: x,
                IsSubAndGenericArguments: x.PropertyType.IsSubclassOfGenericClass(typeof(DatedField<>))
            ))
            .Where(x => x.IsSubAndGenericArguments.IsSubclass)
            .Select(x => {
                var datedField = (DatedField)x.PropertyInfo.GetValue(instance);
            
                return new DatedFieldProperty(
                    x.PropertyInfo,
                    x.IsSubAndGenericArguments.GenericArguments[0],
                    datedField.Code);
            })
            .ToList();
    }
}