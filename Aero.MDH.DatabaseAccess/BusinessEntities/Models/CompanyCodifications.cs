using Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;

namespace Aero.MDH.DatabaseAccess.BusinessEntities.Models;

public class CompanyCodifications : AbstractCodificationHolder
{
    public TextCodificationField DealEFrontCode => GetCodificationField(CompanyCodificationFields.EFront);
}