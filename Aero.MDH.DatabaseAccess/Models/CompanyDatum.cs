using System.ComponentModel.DataAnnotations.Schema;
using Aero.MDH.DatabaseAccess.Models.Contract;

namespace Aero.MDH.DatabaseAccess.Models;

public partial class CompanyDatum : IDataTable
{
    public void SetId(int id)
    {
        CompanyId = id;
    }
}