using DB.Models;
using IRepository.Sys;

namespace Repository.Sys
{
    public class Sys_LogRepository : BaseRepository<SysLog>, ISys_LogRepository
    {
        private readonly QrsfactoryWmsContext _context;
        public Sys_LogRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
    }
}
