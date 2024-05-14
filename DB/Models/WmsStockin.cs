using System;
using System.Collections.Generic;

namespace DB.Models;

public partial class WmsStockin
{
    public long StockInId { get; set; }

    public string? StockInNo { get; set; }

    public long? StockInType { get; set; }

    public long? SupplierId { get; set; }

    public string? OrderNo { get; set; }

    public byte? StockInStatus { get; set; }

    public string? Remark { get; set; }

    public byte? IsDel { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
