namespace Aero.Application.Workers;

public class StringArgument : Argument<string>
{
    public StringArgument(string name, bool isRequired, string description) : base(name, isRequired, description)
    {
    }

    public StringArgument(string name, bool isRequired, string defaultValue, string description) : base(name, isRequired, defaultValue, description)
    {
    }
}