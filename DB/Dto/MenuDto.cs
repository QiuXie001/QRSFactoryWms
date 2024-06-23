using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Dto
{
    internal class MenuDto
    {
        public long MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuUrl { get; set; }
        public string MenuIcon { get; set; }
        public long MenuParent { get; set; }

        public int Sort { get; set; }
        public byte Status { get; set; }
        public string MenuType { get; set; }
        public byte IsDel { get; set; }
        public string Remark { get; set; }

        public long? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
