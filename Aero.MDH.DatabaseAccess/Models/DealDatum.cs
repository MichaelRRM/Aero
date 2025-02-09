namespace Aero.MDH.DatabaseAccess.Models;

public partial class DealDatum : IDataTable
{
    public int Id
    {
        get => DealId;
        set => DealId = value;
    }
}