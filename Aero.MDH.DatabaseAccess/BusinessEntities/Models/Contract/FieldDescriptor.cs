namespace Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;

public record FieldDescriptor<T>(string DataCode);

// to discuss - ideas for what could go in there: 
// public bool IsRequired { get; init; }
// public T? DefaultValue { get; init; }
// public T? MinValue { get; init; }
// public T? MaxValue { get; init; }
// public bool IsReadOnly { get; init; }
// public string? ValidationRegex { get; init; } 
// public string? Format { get; init; } 
// public bool IsCacheable { get; init; }
// public TimeSpan? CacheDuration { get; init; }