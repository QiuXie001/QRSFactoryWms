using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models;

public partial class SysDict
{
    public long DictId { get; set; }

    public string? DictNo { get; set; }

    public string? DictName { get; set; }

    public string? DictType { get; set; }

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

    public virtual ICollection<WmsStockin> Stockins { get; set; } = new List<WmsStockin>();
    public virtual ICollection<WmsStockout> Stockouts { get; set; } = new List<WmsStockout>();

    public virtual ICollection<WmsMaterial> MaterialsType { get; set; } = new List<WmsMaterial>();

    public virtual ICollection<WmsMaterial> MaterialsUnit { get; set; } = new List<WmsMaterial>();
}
