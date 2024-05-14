using System;
using System.Collections.Generic;

namespace DB.Models;

public partial class WmsInventorymove
{
    public long InventorymoveId { get; set; }

    public string? InventorymoveNo { get; set; }

    public long? SourceStoragerackId { get; set; }

    public long? AimStoragerackId { get; set; }

    public byte? Status { get; set; }

    public string? Remark { get; set; }

    public byte? IsDel { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
