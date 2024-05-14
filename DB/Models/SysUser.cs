using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models;

public class SysUser
{
    [Key]
    public long UserId { get; set; }

    [Required]
    [StringLength(50)]
    public string UserName { get; set; }

    [Required]
    [StringLength(50)]
    public string UserNickname { get; set; }

    [Required]
    [StringLength(32)]
    public string Pwd { get; set; }

    public string Sort { get; set; }

    [StringLength(50)]
    public string Email { get; set; }

    [StringLength(20)]
    public string Tel { get; set; }

    [StringLength(12)]
    public string Mobile { get; set; }

    [Required]
    public byte Sex { get; set; }

    [Required]
    public long RoleId { get; set; }
    [Required]
    public long DeptId { get; set; }

    [StringLength(15)]
    public string LoginIp { get; set; }

    public DateTime LoginDate { get; set; }

    public int LoginTime { get; set; }

    public string HeadImg { get; set; }

    [Required]
    public byte IsEabled { get; set; }

    [Required]
    public byte IsDel { get; set; }

    [StringLength(255)]
    public string Remark { get; set; }

    public long CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long ModifiedBy { get; set; }
    public virtual ICollection<SysDept> CreateDepts { get; set; }
    public virtual ICollection<SysDept> ModifiedDepts { get; set; }
    public DateTime? ModifiedDate { get; set; }
    [ForeignKey("DeptId")]
    public virtual SysDept Dept { get; set; }
    [ForeignKey("RoleId")]
    public virtual SysRole Role { get; set; }
    [ForeignKey("ModifiedBy")]
    public virtual SysUser ModifiedByUser { get; set; }
    [ForeignKey("CreateBy")]
    public virtual SysUser CreateByUser { get; set; }

}

