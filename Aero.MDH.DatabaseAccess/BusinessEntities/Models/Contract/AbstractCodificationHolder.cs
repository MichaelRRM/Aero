namespace Aero.MDH.DatabaseAccess.BusinessEntities.Models.Contract;

public abstract class AbstractCodificationHolder
{
    private readonly Dictionary<string, CodificationField> _fields = new();

    protected TextCodificationField GetCodificationField(FieldDescriptor<string?> descriptor)
    {
        var key = descriptor.DataCode;
        if (!_fields.ContainsKey(key))
        {
            _fields[key] = new TextCodificationField(descriptor);
        }
        return (TextCodificationField)_fields[key];
    }
    
    protected NumberCodificationField GetCodificationField(FieldDescriptor<int?> descriptor)
    {
        var key = descriptor.DataCode;
        if (!_fields.ContainsKey(key))
        {
            _fields[key] = new NumberCodificationField(descriptor);
        }
        return (NumberCodificationField)_fields[key];
    }
}