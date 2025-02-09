using System;
using System.Collections.Generic;

namespace Aero.MDH.DatabaseAccess.Models;

public partial class GlobalIdMaster : IAuditable
{
    public int GlobalId { get; set; }

    public string ObjectType { get; set; } = null!;

    public DateTime AuditCreateDate { get; set; }

    public string AuditCreateUser { get; set; } = null!;

    public DateTime? AuditUpdateDate { get; set; }

    public string? AuditUpdateUser { get; set; }

    public virtual ICollection<Codification> Codifications { get; set; } = new List<Codification>();

    public virtual CompanyIdMaster? CompanyIdMaster { get; set; }

    public virtual DealIdMaster? DealIdMaster { get; set; }
}
