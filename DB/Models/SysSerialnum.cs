using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models;

public partial class SysSerialnum
{
    public int SerialNumberId { get; set; }

    public string? SerialNumber { get; set; }

    public int? SerialCount { get; set; }

    public string? TableName { get; set; }

    public string? Prefix { get; set; }

    public int? Digit { get; set; }

    public int? Mantissa { get; set; }

    public byte IsDel { get; set; }

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
