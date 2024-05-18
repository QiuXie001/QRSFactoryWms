using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace DB.Models
{
    public class SysIdentity
    {
        [Key]
        public long Id { get; set; }
        public string Token { get; set; }

        public DateTime GeneratedTime { get; set; }

        public DateTime ExpirationTime { get; set; }

        public long UserId { get; set; }

        [StringLength(15)]
        public string LoginIp { get; set; }
        public byte IsEabled {  get; set; }

        [ForeignKey("UserId")]
        public SysUser User { get; set; }
    }
}
