using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Dto
{
    public class DeliveryDto
    {
        public long DeliveryId { get; set; }

        public long? StockOutId { get; set; }

        public long? CarrierId { get; set; }

        public string? Remark { get; set; }

        public byte? IsDel { get; set; }

        public long? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? DeliveryDate { get; set; }
    }
}
