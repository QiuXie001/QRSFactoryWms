﻿using DB.Models;
using IServices;
using Qiu.Utils.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Sys
{
    public interface ISys_DeptService : IBaseService<SysDept>
    {
        public Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap);
    }
}