using DB.Models;
using IResponsitory.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.Sys
{
    public class Sys_MenuResponsitory : BaseResponsitory<SysMenu>, ISys_MenuResponsitory
    {
        public Sys_MenuResponsitory(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
