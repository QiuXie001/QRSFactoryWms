using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Dto
{
    public class Invmovedetail
    {
        public long? MaterialId { get; set; }
        public string MaterialNo { get; set; }
        public string MaterialName { get; set; }
        public long? MoveDetailId { get; set; }
        public decimal? ActQty { get; set; }
        public decimal? Qty { get; set; }
        public long? AuditinId { get; set; }
        public DateTime? AuditinTime { get; set; }
        public long? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? InventorymoveId { get; set; }
        public int? IsDel { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal? PlanQty { get; set; }
        public string Remark { get; set; }
        public int? Status { get; set; }
    }

}
