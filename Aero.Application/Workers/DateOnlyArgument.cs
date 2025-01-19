namespace Aero.Application.Workers;

public class DateOnlyArgument : Argument<DateOnly>
{
    public DateOnlyArgument(string name, bool isRequired, string description) : base(name, isRequired, description)
    {
    }

    public DateOnlyArgument(string name, bool isRequired, DateOnly defaultValue, string description) : base(name, isRequired, defaultValue, description)
    {
    }
}