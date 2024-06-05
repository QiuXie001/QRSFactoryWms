using DB.Models;
using IRepository.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Sys
{
    public class Sys_MenuRepository : BaseRepository<SysMenu>, ISys_MenuRepository
    {
        public Sys_MenuRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
