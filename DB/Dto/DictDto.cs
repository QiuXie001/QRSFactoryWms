using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Dto
{
    public class DictDto
    {
        public long DictId { get; set; }

        public string? DictNo { get; set; }

        public string? DictName { get; set; }

        public string? DictType { get; set; }

        public byte IsDel { get; set; }

        public string? Remark { get; set; }

        public long? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
