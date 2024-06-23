using DB.Models;
using Qiu.Utils.Table;

namespace IServices.Sys
{
    public interface ISys_DeptService : IBaseService<SysDept>
    {
        Task<Dictionary<long, string>> GetDeptList();
        public Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap);

        Task<string> GetDeptNameById(long deptId);
    }
}
