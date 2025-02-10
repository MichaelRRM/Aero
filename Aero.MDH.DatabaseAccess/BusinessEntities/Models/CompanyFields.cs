using Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Models;

public static class CompanyFields
{
    public static readonly FieldDescriptor<string?> Name = new("name");
    public static readonly FieldDescriptor<int?> EmployeeCount = new("employee_count");
}