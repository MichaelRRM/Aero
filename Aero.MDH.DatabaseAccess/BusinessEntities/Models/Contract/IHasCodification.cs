namespace Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;

public interface IHasCodification<T>
{
    T Codifications { get; }
}