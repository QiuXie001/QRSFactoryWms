using System.ComponentModel.DataAnnotations.Schema;

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

    [ForeignKey("CreateBy")]
    public SysUser CreateByUser { get; set; }

    [ForeignKey("ModifiedBy")]
    public SysUser ModifiedByUser { get; set; }

    [ForeignKey("RoleId")]
    public SysRole Role { get; set; }

    [ForeignKey("MenuId")]
    public SysMenu Menu { get; set; }
}
