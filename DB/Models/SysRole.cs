using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DB.Models;

public class SysRole
{
    [Key]
    public long RoleId { get; set; }

    [Required]
    [StringLength(50)]
    public string RoleName { get; set; }

    [Required]
    [StringLength(10)]
    public string RoleType { get; set; }

    [Required]
    public byte IsDel { get; set; }

    [StringLength(255)]
    public string Remark { get; set; }

    public virtual ICollection<SysUser> Users { get; set; }
    public virtual ICollection<SysRoleMenu> RoleMenus { get; set; }
    
    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
    [ForeignKey("CreateBy")]
    public virtual SysUser CreateByUser { get; set; }
    [ForeignKey("ModifiedBy")]
    public virtual SysUser ModifiedByUser { get; set; }
}

