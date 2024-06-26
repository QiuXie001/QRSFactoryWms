using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Dto
{
    public class CustomerDto
    {
        public long CustomerId { get; set; }

        public string CustomerNo { get; set; } = null!;

        public string CustomerName { get; set; } = null!;

        public string? Address { get; set; }

        public string? Tel { get; set; }

        public string? CustomerPerson { get; set; }

        public string? CustomerLevel { get; set; }

        public string? Email { get; set; }

        public string? Remark { get; set; }

        public byte? IsDel { get; set; }
    }
}
