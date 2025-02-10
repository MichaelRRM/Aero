using Aero.MDH.DatabaseAccess.Models.Contract;

namespace Aero.MDH.DatabaseAccess.Models;

public partial class DealIdMaster : IAuditable
{
    public int DealId { get; set; }

    public DateTime AuditCreateDate { get; set; }

    public string AuditCreateUser { get; set; } = null!;

    public DateTime? AuditUpdateDate { get; set; }

    public string? AuditUpdateUser { get; set; }

    public virtual GlobalIdMaster Deal { get; set; } = null!;

    public virtual ICollection<DealDatum> DealData { get; set; } = new List<DealDatum>();
}
