using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models;

public partial class SysIdentity
{
    public long Id { get; set; }

    public string Token { get; set; } = null!;

    public DateTime GeneratedTime { get; set; }

    public DateTime ExpirationTime { get; set; }

    public long UserId { get; set; }

    public byte? IsEabled { get; set; }

    public string LoginIp { get; set; } = null!;


    [ForeignKey("UserId")]
    public SysUser User { get; set; }
}
