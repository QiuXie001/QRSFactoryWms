using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models;

public partial class WmsStockoutdetail
{
    public long StockOutDetailId { get; set; }

    public long? StockOutId { get; set; }

    public byte? Status { get; set; }

    public long? MaterialId { get; set; }

    public decimal? PlanOutQty { get; set; }

    public decimal? ActOutQty { get; set; }

    public long? StoragerackId { get; set; }

    public long? AuditinId { get; set; }

    public DateTime? AuditinTime { get; set; }

    public byte? IsDel { get; set; }

    public string? Remark { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("StockOutId")]
    public WmsStockout Stockout { get; set; }

    [ForeignKey("MaterialId")]
    public WmsMaterial Material { get; set; }

    [ForeignKey("StoragerackId")]
    public WmsStoragerack Storagerack { get; set; }

    [ForeignKey("AuditinId")]
    public SysUser AuditinByUser { get; set; }

    [ForeignKey("CreateBy")]
    public SysUser CreateByUser { get; set; }

    [ForeignKey("ModifiedBy")]
    public SysUser ModifiedByUser { get; set; }
}
