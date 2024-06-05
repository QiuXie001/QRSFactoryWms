using DB.Models;
using IRepository.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Sys
{
    public class Sys_IdentityRepository : BaseRepository<SysIdentity>, ISys_IdentityRepository
    {
        public Sys_IdentityRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
