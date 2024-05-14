using System;
using System.Collections.Generic;

namespace DB.Models;

public partial class WmsInventory
{
    public long InventoryId { get; set; }

    public long? MaterialId { get; set; }

    public long? StoragerackId { get; set; }

    public decimal? Qty { get; set; }

    public string? Remark { get; set; }

    public byte IsDel { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
