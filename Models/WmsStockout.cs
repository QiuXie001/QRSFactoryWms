using System;
using System.Collections.Generic;

namespace DB.Models;

public partial class WmsStockout
{
    public long StockOutId { get; set; }

    public string? StockOutNo { get; set; }

    public string? OrderNo { get; set; }

    public long? StockOutType { get; set; }

    public long? CustomerId { get; set; }

    public byte? StockOutStatus { get; set; }

    public string? Remark { get; set; }

    public byte? IsDel { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
