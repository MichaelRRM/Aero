using System;
using System.Collections.Generic;

namespace MDH.DatabaseAccess.Models;

public partial class DealDatum
{
    public int? DealId { get; set; }

    public DateTime? ValueDate { get; set; }

    public string? DataCode { get; set; }

    public string? DataValueTxt { get; set; }

    public decimal? DataValueNum { get; set; }

    public int? DataValueInt { get; set; }

    public int? DataValueBit { get; set; }
}
