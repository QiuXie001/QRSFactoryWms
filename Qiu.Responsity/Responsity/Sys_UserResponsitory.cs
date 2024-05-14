using DB.Models;
using IResponsitory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory
{
    public class Sys_UserRespository : BaseResponsitory<SysUser>, ISys_UserResponsitory
    {
        public Sys_UserRespository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
