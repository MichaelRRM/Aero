namespace Aero.Base.Extensions;

public static class TypeExtensions
{
    public static (bool IsSubclass, Type[]? GenericArguments) IsSubclassOfGenericClass(this Type? typeToCheck,
        Type genericBaseClass)
    {
        while (typeToCheck != null && typeToCheck != typeof(object))
        {
            var currentType = typeToCheck.IsGenericType ? typeToCheck.GetGenericTypeDefinition() : typeToCheck;
            if (genericBaseClass == currentType)
            {
                return (true, typeToCheck.GetGenericArguments());
            }
            typeToCheck = typeToCheck.BaseType;
        }
        return (false, null);
    }
}