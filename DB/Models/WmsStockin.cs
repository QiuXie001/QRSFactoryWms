using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models;

public partial class WmsStockin
{
    public long StockInId { get; set; }

    public string? StockInNo { get; set; }

    public long? StockInTypeId { get; set; }

    public long? SupplierId { get; set; }

    public string? OrderNo { get; set; }

    public byte? StockInStatus { get; set; }

    public string? Remark { get; set; }

    public byte? IsDel { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("StockInTypeId")]
    public SysDict StockInType { get; set; }
    [ForeignKey("SupplierId")]
    public WmsSupplier Supplier { get; set; }

    [ForeignKey("CreateBy")]
    public SysUser CreateByUser { get; set; }

    [ForeignKey("ModifiedBy")]
    public SysUser ModifiedByUser { get; set; }

    public virtual ICollection<WmsStockindetail> Stockindetails { get; set; } = new List<WmsStockindetail>();

}
