using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Dto
{
    public class WarehouseDto
    {
        public long WarehouseId { get; set; }

        public string WarehouseNo { get; set; } = null!;

        public string WarehouseName { get; set; } = null!;

        public byte? IsDel { get; set; }

        public string? Remark { get; set; }
    }
}
