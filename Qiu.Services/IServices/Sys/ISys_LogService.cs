using DB.Models;
using Qiu.Utils.Table;

namespace IServices.Sys
{
    public interface ISys_LogService : IBaseService<SysLog>
    {

        public Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap);
        public Task<string> EChart(Bootstrap.BootstrapParams bootstrap);
    }
}
