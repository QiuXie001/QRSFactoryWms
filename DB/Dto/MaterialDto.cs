using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Dto
{
    public class MaterialDto
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

    }
}
