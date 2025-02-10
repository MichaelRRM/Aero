using System.Dynamic;

namespace Aero.MDH.DatabaseAccess.Models.Contract;

public interface IDatedTable : IAuditable
{
    void SetId(int id);
    DateOnly ValueDate { get; set; }
}