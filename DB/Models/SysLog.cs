using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DB.Models;

public class SysLog
{
    [Key]
    public long LogId { get; set; }

    [Required]
    [StringLength(10)]
    public string LogType { get; set; }

    public string Description { get; set; }

    [StringLength(150)]
    public string Url { get; set; }

    [StringLength(255)]
    public string Browser { get; set; }


    [Required]
    public long? CreateBy { get; set; }

    [Required]
    public DateTime CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    [StringLength(15)]
    public string LogIp { get; set; }
    
    [ForeignKey("CreateBy")]
    public SysUser CreateByUser { get; set; }
    
    [ForeignKey("ModifiedBy")]
    public SysUser ModifiedByUser { get; set; }
}
