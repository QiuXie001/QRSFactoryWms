using DB.Models;
using Qiu.Utils.Table;

namespace IServices.Wms
{
    public interface IWms_InventorymoveService : IBaseService<WmsInventorymove>
    {
        Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap);

        Task<bool> Auditin(long userId, long InventorymoveId);

        Task<string> PrintList(string InventorymoveId);
    }
}