using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models;

public partial class WmsInventorymove
{
    public long InventorymoveId { get; set; }

    public string? InventorymoveNo { get; set; }

    public long? SourceStoragerackId { get; set; }

    public long? AimStoragerackId { get; set; }

    public byte? Status { get; set; }

    public string? Remark { get; set; }

    public byte? IsDel { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<WmsInvmovedetail> Invmovedetails { get; set; } = new List<WmsInvmovedetail>();


    [ForeignKey("SourceStoragerackId")]
    public WmsStoragerack SourceStoragerack { get; set; }

    [ForeignKey("AimStoragerackId")]
    public WmsStoragerack AimStoragerack { get; set; }

    [ForeignKey("CreateBy")]
    public SysUser CreateByUser { get; set; }

    [ForeignKey("ModifiedBy")]
    public SysUser ModifiedByUser { get; set; }
}
