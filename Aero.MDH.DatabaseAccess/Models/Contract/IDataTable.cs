namespace Aero.MDH.DatabaseAccess.Models.Contract;

public interface IDataTable : IDatedTable
{
    string DataCode { get; set; }

    string? DataValueTxt { get; set; }

    long? DataValueInt { get; set; }

    DateOnly? DataValueDate { get; set; }
    
    bool? DataValueBit { get; set; }
}