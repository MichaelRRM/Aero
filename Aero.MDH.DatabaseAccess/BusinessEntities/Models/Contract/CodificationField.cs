namespace Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;

public abstract class CodificationField
{
    public abstract string Code { get; }
}

public abstract class CodificationField<T> : CodificationField
{
    protected CodificationField(FieldDescriptor<T> descriptor)
    {
        Code = descriptor.DataCode;
    }

    public override string Code { get; }

    public T? Value { get; private set; }

    public bool HasValue { get; private set; }

    public void Feed(T value)
    {
        HasValue = true;
        Value = value;
    }
}

public class TextCodificationField : CodificationField<string?>
{
    public TextCodificationField(FieldDescriptor<string?> descriptor) : base(descriptor)
    {
    }
}

public class NumberCodificationField : CodificationField<int?>
{
    public NumberCodificationField(FieldDescriptor<int?> descriptor) : base(descriptor)
    {
    }
}