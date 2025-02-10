using System.ComponentModel.DataAnnotations.Schema;
using Aero.MDH.DatabaseAccess.Models.Contract;

namespace Aero.MDH.DatabaseAccess;

public partial class CompanyDatum : IDataTable
{
    [NotMapped]
    public int Id
    {
        get => CompanyId;
        set => CompanyId = value;
    }
}