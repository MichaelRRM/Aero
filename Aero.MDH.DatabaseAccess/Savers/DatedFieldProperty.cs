using System.Reflection;

namespace Aero.MDH.DatabaseAccess.Savers;

public class DatedFieldProperty
{
    public DatedFieldProperty(PropertyInfo propertyInfo, Type genericArgument, string code)
    {
        PropertyInfo = propertyInfo;
        GenericArgument = genericArgument;
        Code = code;
    }

    public PropertyInfo PropertyInfo { get; }
    public Type GenericArgument { get; set; }
    public string Code { get; set; }
}