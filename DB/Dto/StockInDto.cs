using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Dto
{
    public class StockInDto
    {
        public long StockInId { get; set; }

        public string? StockInNo { get; set; }

        public long? StockInTypeId { get; set; }

        public long? SupplierId { get; set; }

        public string? OrderNo { get; set; }

        public byte? StockInStatus { get; set; }

        public string? Remark { get; set; }

        public byte? IsDel { get; set; }
    }
}
