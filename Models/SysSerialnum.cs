using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DB.Models;

public class SysSerialNum
{
    [Key]
    public int SerialNumberId { get; set; }

    [Required]
    [StringLength(30)]
    public string SerialNumber { get; set; }

    public int SerialCount { get; set; }

    [StringLength(50)]
    public string TableName { get; set; }

    [StringLength(10)]
    public string Prefix { get; set; }

    public int Digit { get; set; }

    public int Mantissa { get; set; }

    [Required]
    public byte IsDel { get; set; }

    [StringLength(255)]
    public string Remark { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
    
    [ForeignKey("CreateBy")]
    public SysUser CreateByUser { get; set; }
    
    [ForeignKey("ModifiedBy")]
    public SysUser ModifiedByUser { get; set; }
}

