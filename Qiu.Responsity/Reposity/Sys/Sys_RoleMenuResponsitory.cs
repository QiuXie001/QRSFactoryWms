using DB.Models;
using IRepository.Sys;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Sys
{
    public class Sys_RoleMenuRepository : BaseRepository<SysRoleMenu>, ISys_RoleMenuRepository
    {
        public Sys_RoleMenuRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
