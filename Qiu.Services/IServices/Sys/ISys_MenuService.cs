using DB.Models;
using Qiu.Utils.Table;

namespace IServices.Sys
{
    public interface ISys_MenuService : IBaseService<SysMenu>
    {
        public Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap);
    }
}
