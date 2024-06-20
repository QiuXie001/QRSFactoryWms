using DB.Models;
using IRepository.Sys;

namespace Repository.Sys
{
    public class Sys_IdentityRepository : BaseRepository<SysIdentity>, ISys_IdentityRepository
    {
        public Sys_IdentityRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
