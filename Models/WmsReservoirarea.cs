using System;
using System.Collections.Generic;

namespace DB.Models;

public partial class WmsReservoirarea
{
    public long ReservoirAreaId { get; set; }

    public string ReservoirAreaNo { get; set; } = null!;

    public string ReservoirAreaName { get; set; } = null!;

    public long WarehouseId { get; set; }

    public string? Remark { get; set; }

    public byte? IsDel { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
