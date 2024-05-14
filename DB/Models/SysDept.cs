using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DB.Models;

public partial class SysDept
{
    [Key]
    public long DeptId { get; set; }

    [Required]
    [StringLength(20)]
    public string? DeptNo { get; set; }

    [Required]
    [StringLength(50)]
    public string? DeptName { get; set; }

    [Required]
    public byte IsDel { get; set; }

    [StringLength(255)]
    public string? Remark { get; set; }

    public virtual ICollection<SysUser> Users { get; set; }

    public long CreateBy { get; set; }

    [Required]
    public DateTime CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
    
    [ForeignKey("CreateBy")]
    public virtual SysUser CreateByUser { get; set; }

    [ForeignKey("ModifiedBy")]
    public virtual SysUser ModifiedByUser { get; set; }
}
