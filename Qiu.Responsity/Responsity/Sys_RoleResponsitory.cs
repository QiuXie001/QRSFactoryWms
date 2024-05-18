using DB.Models;
using IResponsitory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory
{
    public class Sys_RoleResponsitory : BaseResponsitory<SysRole>, ISys_RoleResponsitory
    {
        public Sys_RoleResponsitory(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
