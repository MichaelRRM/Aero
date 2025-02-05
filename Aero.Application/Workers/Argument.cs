using Microsoft.Extensions.Configuration;

namespace Aero.Application.Workers;

public abstract class Argument<T>
{
    protected Argument(string name, bool isRequired, string description)
    {
        Name = name;
        IsRequired = isRequired;
        HasDefaultValue = false;
        Description = description;
    }

    protected Argument(string name, bool isRequired, T defaultValue, string description)
    {
        Name = name;
        IsRequired = isRequired;
        HasDefaultValue = true;
        DefaultValue = defaultValue;
        Description = description;
    }

    public string Name { get; }
    public bool IsRequired { get; }
    public bool HasDefaultValue { get; }
    public T? DefaultValue { get; }
    public string Description { get; }

    public T? GetValue(IConfiguration configuration)
    {
        var argumentAsString = configuration.GetValue<T>(Name);
        var isArgumentDefined = argumentAsString != null;

        if (isArgumentDefined)
        {
            return configuration.GetValue<T>(Name) ??
                   throw new ArgumentException($"Couldn't parse value {argumentAsString} for argument {Name}");
        }

        if (HasDefaultValue)
        {
            return DefaultValue;
        }

        if (IsRequired)
        {
            throw new ArgumentException($"Required argument {Name} is missing.");
        }

        return default;
    }
}