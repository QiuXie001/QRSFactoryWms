using System;
using System.Collections.Generic;

namespace DB.Models;

public partial class SysRole
{
    public long RoleId { get; set; }

    public string? RoleName { get; set; }

    public string? RoleType { get; set; }

    public byte IsDel { get; set; }

    public string? Remark { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
