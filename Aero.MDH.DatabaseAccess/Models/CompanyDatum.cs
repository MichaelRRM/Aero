using Aero.MDH.DatabaseAccess.Models.Base;

namespace Aero.MDH.DatabaseAccess.Models;

public partial class CompanyDatum : IDataTable
{
    public int Id
    {
        get => CompanyId;
        set => CompanyId = value;
    }
}