using DB.Models;
using IRepository.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Sys
{
    public class Sys_DeptRepository : BaseRepository<SysDept>, ISys_DeptRepository
    {
        public Sys_DeptRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
