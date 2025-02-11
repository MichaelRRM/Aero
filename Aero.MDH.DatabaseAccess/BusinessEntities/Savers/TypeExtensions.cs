using System.Collections.Concurrent;
using System.Reflection;
using Aero.Base.Extensions;
using Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;
using Aero.MDH.DatabaseAccess.BusinessEntities.Savers.Contract;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Savers;

public static class TypeExtensions
{
    private static readonly ConcurrentDictionary<Type, List<GenericFieldProperty>> TypeToDatedFieldProperty = new();
    
    public static List<GenericFieldProperty> GetDatedFieldProperties(this Type typeToSearch)
    {
        return TypeToDatedFieldProperty.GetOrAdd(typeToSearch, HandleGetDatedFieldProperties);
    }
    
    private static List<GenericFieldProperty> HandleGetDatedFieldProperties(Type entityType)
    {
        return entityType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(x => (PropertyInfo: x, IsSubAndGenericArguments: x.PropertyType.IsSubclassOfGenericClass(typeof(DatedField<>))))
            .Where(propertyInfoAndIsSubAndGenericArgument => propertyInfoAndIsSubAndGenericArgument.IsSubAndGenericArguments.IsSubclass)
            .Select(propertyInfoAndIsSubAndGenericArgument => 
            {
                var instance = Activator.CreateInstance(entityType);
                var field = propertyInfoAndIsSubAndGenericArgument.PropertyInfo.GetValue(instance);
                var dataCode = field?.GetType()
                    .GetProperty("Code")
                    ?.GetValue(field)  as string;

                var underlyingType = propertyInfoAndIsSubAndGenericArgument.IsSubAndGenericArguments.GenericArguments?[0];
                if (underlyingType == null)
                {
                    return null;
                }
                
                return new GenericFieldProperty(propertyInfoAndIsSubAndGenericArgument.PropertyInfo, underlyingType, dataCode ?? string.Empty);
            })
            .OfType<GenericFieldProperty>()
            .ToList();
    }
    
    public static List<GenericFieldProperty> GetCodificationFieldProperties(this Type typeToSearch)
    {
        return TypeToDatedFieldProperty.GetOrAdd(typeToSearch, HandleGetCodificationFieldProperties);
    }
    
    private static List<GenericFieldProperty> HandleGetCodificationFieldProperties(Type entityType)
    {
        return entityType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(x => (PropertyInfo: x, IsSubAndGenericArguments: x.PropertyType.IsSubclassOfGenericClass(typeof(CodificationField<>))))
            .Where(propertyInfoAndIsSubAndGenericArgument => propertyInfoAndIsSubAndGenericArgument.IsSubAndGenericArguments.IsSubclass)
            .Select(propertyInfoAndIsSubAndGenericArgument => 
            {
                var instance = Activator.CreateInstance(entityType);
                var field = propertyInfoAndIsSubAndGenericArgument.PropertyInfo.GetValue(instance);
                var dataCode = field?.GetType()
                    .GetProperty("Code")
                    ?.GetValue(field)  as string;

                var underlyingType = propertyInfoAndIsSubAndGenericArgument.IsSubAndGenericArguments.GenericArguments?[0];
                if (underlyingType == null)
                {
                    return null;
                }
                
                return new GenericFieldProperty(propertyInfoAndIsSubAndGenericArgument.PropertyInfo, underlyingType, dataCode ?? string.Empty);
            })
            .OfType<GenericFieldProperty>()
            .ToList();
    }
}