using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DB.Models;

public class SysRoleMenu
{
    [Key]
    public long RoleMenuId { get; set; }

    [Required]
    public long RoleId { get; set; }

    [Required]
    public long MenuId { get; set; }
    [ForeignKey("RoleId")]
    public SysRole Role { get; set; }
    [ForeignKey("MenuId")]
    public SysMenuWms Menu { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
    
    [ForeignKey("CreateBy")]
    public SysUser CreateByUser { get; set; }
    
    [ForeignKey("ModifiedBy")]
    public SysUser ModifiedByUser { get; set; }
}

