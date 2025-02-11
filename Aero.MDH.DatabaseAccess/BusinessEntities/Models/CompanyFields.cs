using Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Models;

public static class CompanyFields
{
    public static readonly FieldDescriptor<string?> Name = new("name");
}

public static class CompanyCodificationFields
{
    public static readonly FieldDescriptor<string?> EFront = new("COMPANY_EFRONT_CODE");
}