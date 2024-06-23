using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Dto
{
    public class SupplierDto
    {
        public long SupplierId { get; set; }

        public string SupplierNo { get; set; } = null!;

        public string SupplierName { get; set; } = null!;

        public string? Address { get; set; }

        public string? Tel { get; set; }

        public string? SupplierPerson { get; set; }

        public string? SupplierLevel { get; set; }

        public string? Email { get; set; }

        public string? Remark { get; set; }

        public byte? IsDel { get; set; }

        public long? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
