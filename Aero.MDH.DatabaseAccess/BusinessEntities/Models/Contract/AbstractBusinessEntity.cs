namespace Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;

public abstract class AbstractBusinessEntity
{
    public int Id { get; set; }
    
    private readonly Dictionary<string, DatedField> _fields = new();

    protected StringDatedField GetField(FieldDescriptor<string?> descriptor)
    {
        var key = descriptor.DataCode;
        if (!_fields.ContainsKey(key))
        {
            _fields[key] = new StringDatedField(descriptor);
        }
        return (StringDatedField)_fields[key];
    }
    
    protected DateDatedField GetField(FieldDescriptor<DateTime?> descriptor)
    {
        var key = descriptor.DataCode;
        if (!_fields.ContainsKey(key))
        {
            _fields[key] = new DateDatedField(descriptor);
        }
        return (DateDatedField)_fields[key];
    }
    
    protected BooleanDatedField GetField(FieldDescriptor<bool?> descriptor)
    {
        var key = descriptor.DataCode;
        if (!_fields.ContainsKey(key))
        {
            _fields[key] = new BooleanDatedField(descriptor);
        }
        return (BooleanDatedField)_fields[key];
    }
    
    protected IntDatedField GetField(FieldDescriptor<int?> descriptor)
    {
        var key = descriptor.DataCode;
        if (!_fields.ContainsKey(key))
        {
            _fields[key] = new IntDatedField(descriptor);
        }
        return (IntDatedField)_fields[key];
    }
}