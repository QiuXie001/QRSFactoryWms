using DB.Models;
using IResponsitory.Sys;
using Responsitory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.Sys
{
    public class Sys_RoleMenuResponsitory : BaseResponsitory<SysRoleMenu>, ISys_RoleMenuResponsitory
    {
        public Sys_RoleMenuResponsitory(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
