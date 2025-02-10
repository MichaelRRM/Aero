using System.Collections.Concurrent;
using System.Reflection;
using Aero.Base.Extensions;
using Aero.MDH.DatabaseAccess.BusinessEntities.Base;
using Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Base;

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
        return typeToSearch
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(x => (
                PropertyInfo: x,
                IsSubAndGenericArguments: x.PropertyType.IsSubclassOfGenericClass(typeof(DatedField<>))
            ))
            .Where(x => x.IsSubAndGenericArguments.IsSubclass)
            .Select(x => new DatedFieldProperty(
                x.PropertyInfo,
                x.IsSubAndGenericArguments.GenericArguments[0],
                ((DatedField) Activator.CreateInstance(x.PropertyInfo.PropertyType)).Code))
            .ToList();
    }
}