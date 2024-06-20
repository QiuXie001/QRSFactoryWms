using DB.Models;
using IRepository.Sys;

namespace Repository.Sys
{
    public class Sys_DeptRepository : BaseRepository<SysDept>, ISys_DeptRepository
    {
        public Sys_DeptRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
