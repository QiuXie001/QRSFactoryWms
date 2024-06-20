using DB.Models;
using IRepository.Sys;

namespace Repository.Sys
{
    public class Sys_MenuRepository : BaseRepository<SysMenu>, ISys_MenuRepository
    {
        public Sys_MenuRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
