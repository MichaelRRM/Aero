namespace Aero.MDH.DatabaseAccess.Models.Contract;

public interface IDatedTable : IAuditable
{
    int Id { get; set; }

    DateOnly ValueDate { get; set; }
}