using DB.Models;
using IResponsitory;
using Responsitory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory
{
    public class Sys_IdentityResponsitory : BaseResponsitory<SysIdentity>, ISys_IdentityResponsitory
    {
        public Sys_IdentityResponsitory(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
