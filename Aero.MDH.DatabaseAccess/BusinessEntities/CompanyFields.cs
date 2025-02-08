using Aero.MDH.DatabaseAccess.BusinessEntities.Base;

namespace Aero.MDH.DatabaseAccess.BusinessEntities;

public static class CompanyFields
{
    public static readonly FieldDescriptor<string?> Name = new("name");
    public static readonly FieldDescriptor<int?> EmployeeCount = new("employee_count");
}