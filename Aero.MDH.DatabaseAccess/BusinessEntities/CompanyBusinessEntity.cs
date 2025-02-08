using Aero.MDH.DatabaseAccess.BusinessEntities.Base;

namespace Aero.MDH.DatabaseAccess.BusinessEntities;

public class CompanyBusinessEntity : AbstractBusinessEntity
{
    public int? Id { get; set; }
    public StringDatedField Name => GetField(CompanyFields.Name);
}