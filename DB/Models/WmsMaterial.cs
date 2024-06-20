using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models;

public partial class WmsMaterial
{
    public long MaterialId { get; set; }

    public string MaterialNo { get; set; } = null!;

    public string MaterialName { get; set; } = null!;

    public long? MaterialTypeId { get; set; }

    public long? UnitId { get; set; }

    public long? StoragerackId { get; set; }

    public long? ReservoirAreaId { get; set; }

    public long? WarehouseId { get; set; }

    public decimal? Qty { get; set; }

    public decimal? ExpiryDate { get; set; }

    public byte? IsDel { get; set; }

    public string? Remark { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("MaterialTypeId")]
    public SysDict MaterialType { get; set; }

    [ForeignKey("UnitId")]
    public SysDict Unit { get; set; }

    [ForeignKey("StoragerackId")]
    public WmsStoragerack Storagerack { get; set; }

    [ForeignKey("ReservoirAreaId")]
    public WmsReservoirarea Reservoirarea { get; set; }

    [ForeignKey("WarehouseId")]
    public WmsWarehouse Warehouse { get; set; }

    [ForeignKey("CreateBy")]
    public SysUser CreateByUser { get; set; }

    [ForeignKey("ModifiedBy")]
    public SysUser ModifiedByUser { get; set; }

    public virtual ICollection<WmsInventory> Inventories { get; set; } = new List<WmsInventory>();

    public virtual ICollection<WmsInvmovedetail> Invmovedetails { get; set; } = new List<WmsInvmovedetail>();

    public virtual ICollection<WmsStockindetail> Stockindetails { get; set; } = new List<WmsStockindetail>();

    public virtual ICollection<WmsStockoutdetail> Stockoutdetails { get; set; } = new List<WmsStockoutdetail>();

}
