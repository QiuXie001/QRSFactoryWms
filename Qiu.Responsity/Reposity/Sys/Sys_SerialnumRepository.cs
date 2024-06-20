using DB.Models;
using IRepository.Sys;

namespace Repository.Sys
{
    public class Sys_SerialnumRepository : BaseRepository<SysSerialnum>, ISys_SerialnumRepository
    {
        public Sys_SerialnumRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
