using DB.Models;
using IRepository.Sys;

namespace Repository.Sys
{
    public class Sys_UserRespository : BaseRepository<SysUser>, ISys_UserRepository
    {
        public Sys_UserRespository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
