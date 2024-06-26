using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Dto
{
    public class StoragerackDto
    {
        public long StorageRackId { get; set; }

        public string StorageRackNo { get; set; } = null!;

        public string? StorageRackName { get; set; }

        public long ReservoirAreaId { get; set; }

        public long WarehouseId { get; set; }

        public string? Remark { get; set; }

        public byte? IsDel { get; set; }
    }
}
