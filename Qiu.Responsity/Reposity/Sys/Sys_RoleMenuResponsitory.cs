using DB.Models;
using IRepository.Sys;

namespace Repository.Sys
{
    public class Sys_RoleMenuRepository : BaseRepository<SysRolemenu>, ISys_RoleMenuRepository
    {
        public Sys_RoleMenuRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
