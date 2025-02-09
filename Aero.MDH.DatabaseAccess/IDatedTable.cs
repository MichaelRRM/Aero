namespace Aero.MDH.DatabaseAccess;

public interface IDatedTable : IAuditable
{
    int Id { get; set; }

    DateOnly ValueDate { get; set; }
}