using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Dto
{
    public class StockOutDto
    {
        public long StockOutId { get; set; }

        public string? StockOutNo { get; set; }

        public long? StockOutTypeId { get; set; }

        public long? CustomerId { get; set; }

        public string? OrderNo { get; set; }

        public byte? StockOutStatus { get; set; }

        public string? Remark { get; set; }

        public byte? IsDel { get; set; }
    }
}
