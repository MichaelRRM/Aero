using Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Models;

public class CompanyBusinessEntity : AbstractBusinessEntity, IHasCodification<CompanyCodifications>
{
    public string? CompanyType { get; set; }
    public CompanyCodifications Codifications { get; } = new();
    public StringDatedField Name => GetField(CompanyFields.Name);
}