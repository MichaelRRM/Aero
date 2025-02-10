using Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Models;

public class CompanyBusinessEntity : AbstractBusinessEntity
{
    public string? CompanyType { get; set; }
    public StringDatedField Name => GetField(CompanyFields.Name);
}