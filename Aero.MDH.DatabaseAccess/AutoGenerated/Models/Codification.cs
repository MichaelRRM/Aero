using Aero.MDH.DatabaseAccess.Models.Contract;

namespace Aero.MDH.DatabaseAccess;

public partial class Codification : IAuditable
{
    public int GlobalId { get; set; }

    public string CodificationCode { get; set; } = null!;

    public int VersionId { get; set; }

    public string? CodeTxt { get; set; }

    public int? CodeNum { get; set; }

    public DateTime AuditCreateDate { get; set; }

    public string AuditCreateUser { get; set; } = null!;

    public DateTime? AuditUpdateDate { get; set; }

    public string? AuditUpdateUser { get; set; }

    public virtual GlobalIdMaster Global { get; set; } = null!;
}
