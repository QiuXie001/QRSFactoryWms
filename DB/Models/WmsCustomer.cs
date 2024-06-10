﻿using System;
using System.Collections.Generic;

namespace DB.Models;

public partial class WmsCustomer
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

    public long? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
