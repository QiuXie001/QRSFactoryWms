using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models;

public partial class WmsInventoryrecord
{
    public long InventoryrecordId { get; set; }

    public long? StockInDetailId { get; set; }

    public decimal? Qty { get; set; }

    public string? Remark { get; set; }

    public byte IsDel { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
    
    [ForeignKey("StockInDetailId")]
    public WmsStockindetail Stockindetail { get; set; }

    [ForeignKey("CreateBy")]
    public SysUser CreateByUser { get; set; }

    [ForeignKey("ModifiedBy")]
    public SysUser ModifiedByUser { get; set; }
}
