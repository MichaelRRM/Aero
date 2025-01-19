using Microsoft.Extensions.Configuration;

namespace Aero.Application.Workers;

public abstract class Argument<T>
{
    protected Argument(string name, bool isRequired, string description)
    {
        Name = name;
        IsRequired = isRequired;
        _hasDefaultValue = false;
        Description = description;
    }
    protected Argument(string name, bool isRequired, T defaultValue, string description)
    {
        Name = name;
        IsRequired = isRequired;
        _hasDefaultValue = true;
        DefaultValue = defaultValue;
        Description = description;
    }
    
    private bool _hasDefaultValue;

    public string Name { get; }
    public bool IsRequired { get; }
    public T? DefaultValue { get; }
    public string Description { get; }

    public T? GetValue(IConfiguration configuration)
    {
        var argumentAsString  = configuration.GetValue<T>(Name);
        var isArgumentDefined = argumentAsString != null; 

        if (isArgumentDefined)
        {
            return configuration.GetValue<T>(Name) ?? throw new Exception($"Couldn't parse value {argumentAsString} for argument {Name}");
        }

        if (_hasDefaultValue)
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