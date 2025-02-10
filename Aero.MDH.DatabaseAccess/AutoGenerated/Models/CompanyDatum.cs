using Aero.MDH.DatabaseAccess.Models.Contract;

namespace Aero.MDH.DatabaseAccess.Models;

public partial class CompanyDatum : IAuditable
{
    public int CompanyId { get; set; }

    public DateOnly ValueDate { get; set; }

    public string DataCode { get; set; } = null!;

    public string DataValueTxt { get; set; } = null!;

    public long? DataValueInt { get; set; }

    public DateOnly? DataValueDate { get; set; }

    public bool? DataValueBit { get; set; }

    public DateTime AuditCreateDate { get; set; }

    public string AuditCreateUser { get; set; } = null!;

    public DateTime? AuditUpdateDate { get; set; }

    public string? AuditUpdateUser { get; set; }

    public virtual CompanyIdMaster Company { get; set; } = null!;
}
