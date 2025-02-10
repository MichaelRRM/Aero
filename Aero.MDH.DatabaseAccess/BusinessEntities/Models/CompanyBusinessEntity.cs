using Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Models;

public class CompanyBusinessEntity : AbstractBusinessEntity
{
    public string? CompanyType { get; set; }
    public CompanyCodificationBusinessEntity CompanyCodificationBusinessEntity { get; } = new();
    public StringDatedField Name => GetField(CompanyFields.Name);
}