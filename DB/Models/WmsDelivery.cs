using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models;

public partial class WmsDelivery
{
    public long DeliveryId { get; set; }

    public long? StockOutId { get; set; }

    public long? CarrierId { get; set; }

    public string? Remark { get; set; }

    public byte? IsDel { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public string? TrackingNo { get; set; }

    [ForeignKey("StockOutId")]
    public WmsStockout Stockout { get; set; }
    [ForeignKey("CarrierId")]
    public WmsCarrier Carrier { get; set; }


    [ForeignKey("CreateBy")]
    public SysUser CreateByUser { get; set; }

    [ForeignKey("ModifiedBy")]
    public SysUser ModifiedByUser { get; set; }
}
