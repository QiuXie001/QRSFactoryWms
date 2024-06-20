using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models;

public class SysMenu
{
    [Key]
    public long MenuId { get; set; }

    [Required]
    [StringLength(50)]
    public string MenuName { get; set; }

    [Required]
    [StringLength(50)]
    public string MenuUrl { get; set; }

    [Required]
    [StringLength(50)]
    public string MenuIcon { get; set; }

    [Required]
    public long MenuParent { get; set; }

    public int Sort { get; set; }

    [Required]
    public byte Status { get; set; }

    [StringLength(10)]
    public string MenuType { get; set; }

    [Required]
    public byte IsDel { get; set; }

    [StringLength(255)]
    public string Remark { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<SysRolemenu> Rolemenus { get; set; } = new List<SysRolemenu>();

    [ForeignKey("CreateBy")]
    public SysUser CreateByUser { get; set; }

    [ForeignKey("ModifiedBy")]
    public SysUser ModifiedByUser { get; set; }
}

