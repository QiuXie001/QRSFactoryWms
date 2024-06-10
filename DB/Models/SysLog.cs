using System;
using System.Collections.Generic;

namespace DB.Models;

public partial class SysLog
{
    public long LogId { get; set; }

    public string? LogType { get; set; }

    public string? Description { get; set; }

    public string? Url { get; set; }

    public string? Browser { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? LogIp { get; set; }
}
