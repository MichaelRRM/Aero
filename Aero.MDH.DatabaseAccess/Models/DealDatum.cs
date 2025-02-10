using System.ComponentModel.DataAnnotations.Schema;
using Aero.MDH.DatabaseAccess.Models.Contract;

namespace Aero.MDH.DatabaseAccess.Models;

public partial class DealDatum : IDataTable
{
    public void SetId(int id)
    {
        DealId = id;
    }
}