using System;
using System.Collections.Generic;

namespace DB.Models;

public partial class WmsStoragerack
{
    public long StorageRackId { get; set; }

    public string StorageRackNo { get; set; } = null!;

    public string? StorageRackName { get; set; }

    public long ReservoirAreaId { get; set; }

    public string? Remark { get; set; }

    public byte? IsDel { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public long? WarehouseId { get; set; }
}
