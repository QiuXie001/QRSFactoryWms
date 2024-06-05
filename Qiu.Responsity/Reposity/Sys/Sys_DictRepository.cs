using DB.Models;
using IRepository.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Sys
{
    public class Sys_DictRepository : BaseRepository<SysDict>, ISys_DictRepository
    {
        public Sys_DictRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
