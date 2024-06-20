using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models;

public partial class WmsStockout
{
    public long StockOutId { get; set; }

    public string? StockOutNo { get; set; }

    public string? OrderNo { get; set; }

    public long? StockOutTypeId { get; set; }

    public long? CustomerId { get; set; }

    public byte? StockOutStatus { get; set; }

    public string? Remark { get; set; }

    public byte? IsDel { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("StockOutTypeId")]
    public SysDict StockOutType { get; set; }
    [ForeignKey("CustomerId")]
    public WmsCustomer Customer { get; set; }

    [ForeignKey("CreateBy")]
    public SysUser CreateByUser { get; set; }

    [ForeignKey("ModifiedBy")]
    public SysUser ModifiedByUser { get; set; }

    public virtual ICollection<WmsDelivery> Deliveries { get; set; } = new List<WmsDelivery>();
    public virtual ICollection<WmsStockoutdetail> Stockoutdetails { get; set; } = new List<WmsStockoutdetail>();
}
