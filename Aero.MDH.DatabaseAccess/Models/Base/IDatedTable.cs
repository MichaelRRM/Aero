namespace Aero.MDH.DatabaseAccess.Models.Base;

public interface IDatedTable : IAuditable
{
    int Id { get; set; }

    DateOnly ValueDate { get; set; }
}