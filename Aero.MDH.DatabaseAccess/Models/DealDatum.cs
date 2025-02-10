using Aero.MDH.DatabaseAccess.Models.Contract;

namespace Aero.MDH.DatabaseAccess;

public partial class DealDatum : IDataTable
{
    public int Id
    {
        get => DealId;
        set => DealId = value;
    }
}