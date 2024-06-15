using DB.Models;
using Qiu.Utils.Table;

namespace IServices.Wms
{
    public interface IWms_StoragerackService : IBaseService<WmsStoragerack>
    {
        Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap);
    }
}