using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Dto
{
    public class CarrierDto
    {
        public long CarrierId { get; set; }

        public string CarrierNo { get; set; } = null!;

        public string CarrierName { get; set; } = null!;

        public string? Address { get; set; }

        public string? Tel { get; set; }

        public string? CarrierPerson { get; set; }

        public string? CarrierLevel { get; set; }

        public string? Email { get; set; }

        public string? Remark { get; set; }

        public byte? IsDel { get; set; }

    }
}
