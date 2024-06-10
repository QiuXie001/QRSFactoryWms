using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models;

public partial class SysDept
{
    public long DeptId { get; set; }

    public string? DeptNo { get; set; }

    public string? DeptName { get; set; }

    public byte IsDel { get; set; }

    public string? Remark { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }


    [ForeignKey("CreateBy")]
    public virtual SysUser CreateByUser { get; set; }

    [ForeignKey("ModifiedBy")]
    public virtual SysUser ModifiedByUser { get; set; }
}
