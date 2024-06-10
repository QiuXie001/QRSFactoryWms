using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models;

public partial class WmsMaterial
{
    public long MaterialId { get; set; }

    public string MaterialNo { get; set; } = null!;

    public string MaterialName { get; set; } = null!;

    public long? MaterialType { get; set; }

    public long? Unit { get; set; }

    public long? StoragerackId { get; set; }

    public long? ReservoirAreaId { get; set; }

    public long? WarehouseId { get; set; }

    public decimal? Qty { get; set; }

    public decimal? ExpiryDate { get; set; }

    public byte? IsDel { get; set; }

    public string? Remark { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("CreateBy")]
    public SysUser CreateByUser { get; set; }

    [ForeignKey("ModifiedBy")]
    public SysUser ModifiedByUser { get; set; }
}
