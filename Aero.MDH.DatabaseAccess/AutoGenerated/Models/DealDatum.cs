using System;
using System.Collections.Generic;

namespace Aero.MDH.DatabaseAccess.Models;

public partial class DealDatum : IAuditable
{
    public int? DealId { get; set; }

    public DateTime? ValueDate { get; set; }

    public string? DataCode { get; set; }

    public string? DataValueTxt { get; set; }

    public decimal? DataValueNum { get; set; }

    public int? DataValueInt { get; set; }

    public int? DataValueBit { get; set; }

    public DateTime AuditCreateDate { get; set; }
    public string AuditCreateUser { get; set; } = null!;
    public DateTime? AuditUpdateDate { get; set; }
    public string? AuditUpdateUser { get; set; }
}