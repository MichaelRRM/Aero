﻿using Aero.MDH.DatabaseAccess.BusinessEntities.Base;

namespace Aero.MDH.DatabaseAccess.DataServices.Base;

public class SaveResult<T> where T : AbstractBusinessEntity
{
    public SaveResult(bool success, IReadOnlyCollection<T> savedEntities)
    {
        SavedEntities = savedEntities;
        Success = success;
    }

    public bool Success { get; }
    //public IReadOnlyCollection<ValidationError> ValidationErrors { get; }
    public IReadOnlyCollection<T> SavedEntities { get; }
}

//public record ValidationError(string Field, string Message);