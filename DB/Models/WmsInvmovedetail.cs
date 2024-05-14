using System;
using System.Collections.Generic;

namespace DB.Models;

public partial class WmsInvmovedetail
{
    public long MoveDetailId { get; set; }

    public long? InventorymoveId { get; set; }

    public byte? Status { get; set; }

    public long? MaterialId { get; set; }

    public decimal? PlanQty { get; set; }

    public decimal? ActQty { get; set; }

    public long? AuditinId { get; set; }

    public DateTime? AuditinTime { get; set; }

    public byte? IsDel { get; set; }

    public string? Remark { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
