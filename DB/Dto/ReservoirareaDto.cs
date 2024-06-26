using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Dto
{
    public class ReservoirareaDto
    {
        public long ReservoirAreaId { get; set; }

        public string ReservoirAreaNo { get; set; } = null!;

        public string ReservoirAreaName { get; set; } = null!;

        public long WarehouseId { get; set; }

        public string? Remark { get; set; }

        public byte? IsDel { get; set; }
    }
}
