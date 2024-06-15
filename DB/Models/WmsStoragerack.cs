using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models;

public partial class WmsStoragerack
{
    public long StorageRackId { get; set; }

    public string StorageRackNo { get; set; } = null!;

    public string? StorageRackName { get; set; }

    public long ReservoirAreaId { get; set; }

    public string? Remark { get; set; }

    public byte? IsDel { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public long? WarehouseId { get; set; }

    [ForeignKey("ReservoirAreaId")]
    public WmsReservoirarea Reservoirarea { get; set; }

    [ForeignKey("CreateBy")]
    public SysUser CreateByUser { get; set; }

    [ForeignKey("ModifiedBy")]
    public SysUser ModifiedByUser { get; set; }

    public virtual ICollection<WmsInventory> Inventories { get; set; } = new List<WmsInventory>();
    public virtual ICollection<WmsInventorymove> InventorymoveSource { get; set; } = new List<WmsInventorymove>();

    public virtual ICollection<WmsInventorymove> InventorymoveAim { get; set; } = new List<WmsInventorymove>();

    public virtual ICollection<WmsStockindetail> Stockindetails { get; set; } = new List<WmsStockindetail>();

    public virtual ICollection<WmsStockoutdetail> Stockoutdetails { get; set; } = new List<WmsStockoutdetail>();

    public virtual ICollection<WmsMaterial> Materials { get; set; } = new List<WmsMaterial>();

}
