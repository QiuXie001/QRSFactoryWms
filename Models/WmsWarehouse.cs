using System;
using System.Collections.Generic;

namespace DB.Models;

public partial class WmsWarehouse
{
    public long WarehouseId { get; set; }

    public string WarehouseNo { get; set; } = null!;

    public string WarehouseName { get; set; } = null!;

    public byte? IsDel { get; set; }

    public string? Remark { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
