using System;
using System.Collections.Generic;

namespace Aero.MDH.DatabaseAccess.Models;

public partial class CompanyIdMaster : IAuditable
{
    public int CompanyId { get; set; }

    public string CompanyType { get; set; } = null!;

    public DateTime AuditCreateDate { get; set; }

    public string AuditCreateUser { get; set; } = null!;

    public DateTime? AuditUpdateDate { get; set; }

    public string? AuditUpdateUser { get; set; }

    public virtual GlobalIdMaster Company { get; set; } = null!;

    public virtual ICollection<CompanyDatum> CompanyData { get; set; } = new List<CompanyDatum>();
}
