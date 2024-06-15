using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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

    [ForeignKey("WarehouseId")]
    public WmsWarehouse Warehouse { get; set; }

    [ForeignKey("CreateBy")]
    public SysUser CreateByUser { get; set; }

    [ForeignKey("ModifiedBy")]
    public SysUser ModifiedByUser { get; set; }

    public virtual ICollection<WmsMaterial> Materials { get; set; } = new List<WmsMaterial>();
    public virtual ICollection<WmsStoragerack> Storageracks { get; set; } = new List<WmsStoragerack>();
}
