﻿using DB.Models;
using IResponsitory.Sys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.Sys
{
    public class Sys_LogResponsitory : BaseResponsitory<SysLog>, ISys_LogResponsitory
    {
        private readonly QrsfactoryWmsContext _context;
        public Sys_LogResponsitory(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
    }
}