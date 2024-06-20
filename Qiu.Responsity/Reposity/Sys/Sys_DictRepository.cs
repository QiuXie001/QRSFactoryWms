using DB.Models;
using IRepository.Sys;

namespace Repository.Sys
{
    public class Sys_DictRepository : BaseRepository<SysDict>, ISys_DictRepository
    {
        public Sys_DictRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
