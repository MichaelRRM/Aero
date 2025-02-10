using Aero.MDH.DatabaseAccess.Models.Contract;

namespace Aero.MDH.DatabaseAccess;

public partial class CompanyDatum : IDataTable
{
    public int Id
    {
        get => CompanyId;
        set => CompanyId = value;
    }
}