using DB.Models;
using IRepository.Sys;

namespace Repository.Sys
{
    public class Sys_RoleRepository : BaseRepository<SysRole>, ISys_RoleRepository
    {
        public Sys_RoleRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
