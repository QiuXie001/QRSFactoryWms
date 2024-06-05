using DB.Models;
using IRepository.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Sys
{
    public class Sys_SerialnumRepository : BaseRepository<SysSerialNum>, ISys_SerialnumRepository
    {
        public Sys_SerialnumRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
