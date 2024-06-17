using DB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Dto
{
    public partial class Inventorymove
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

        public virtual ICollection<Invmovedetail> Invmovedetails { get; set; } = new List<Invmovedetail>();
        public WmsStoragerack SourceStoragerack { get; set; }
        public WmsStoragerack AimStoragerack { get; set; }
        public SysUser CreateByUser { get; set; }
        public SysUser ModifiedByUser { get; set; }
    }
}
