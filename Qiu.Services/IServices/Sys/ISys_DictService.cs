using DB.Models;
using Qiu.Utils.Table;

namespace IServices.Sys
{
    public interface ISys_DictService : IBaseService<SysDict>
    {
        public Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap);
        Task<Dictionary<long, string>> GetDictListByType(string type);
    }
}
