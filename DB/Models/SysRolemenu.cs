using System;
using System.Collections.Generic;

namespace DB.Models;

public partial class SysRolemenu
{
    public long RoleMenuId { get; set; }

    public long? RoleId { get; set; }

    public long? MenuId { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
