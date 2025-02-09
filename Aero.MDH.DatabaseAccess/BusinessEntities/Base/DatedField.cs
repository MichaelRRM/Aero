namespace Aero.MDH.DatabaseAccess.BusinessEntities.Base;

public abstract class DatedField
{
    public abstract string Code { get; }
}

public abstract class DatedField<T> : DatedField
{
    protected DatedField(FieldDescriptor<T> descriptor)
    {
        Code = descriptor.DataCode;
    }

    public override string Code { get; }

    private (T value, DateOnly valueDate) _field;

    public T Value => _field.value;
    public DateOnly ValueDate => _field.valueDate;
    public bool HasValue { get; private set; }

    public void Feed(T value, DateOnly valueDate)
    {
        HasValue = true;
        _field.value = value;
        _field.valueDate = valueDate;
    }
}

public class DateDatedField : DatedField<DateTime?>
{
    public DateDatedField(FieldDescriptor<DateTime?> descriptor) : base(descriptor)
    {
    }
}

public class StringDatedField : DatedField<string?>
{
    public StringDatedField(FieldDescriptor<string?> descriptor) : base(descriptor)
    {
    }
}

public class IntDatedField : DatedField<int?>
{
    public IntDatedField(FieldDescriptor<int?> descriptor) : base(descriptor)
    {
    }
}

public class BooleanDatedField : DatedField<bool?>
{
    public BooleanDatedField(FieldDescriptor<bool?> descriptor) : base(descriptor)
    {
    }
}