using Aero.MDH.DatabaseAccess.BusinessEntities.Base;

namespace Aero.MDH.DatabaseAccess.DataServices.Base;

public class SaveResult<T> where T : AbstractBusinessEntity
{
    public bool Success { get; }
    public IReadOnlyCollection<string> Warnings { get; }
    public IReadOnlyCollection<ValidationError> ValidationErrors { get; }
    public int EntitiesSaved { get; }
    public IReadOnlyCollection<T> SavedEntities { get; }
}

public record ValidationError(string Field, string Message);