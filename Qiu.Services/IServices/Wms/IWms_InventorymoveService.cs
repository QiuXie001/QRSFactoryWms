using DB.Models;
using Qiu.Utils.Table;

namespace IServices.Wms
{
    public interface IWms_InventorymoveService : IBaseService<WmsInventorymove>
    {
        Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap);

        Task<bool> AuditinAsync(long userId, long InventorymoveId);

        string PrintList(long InventorymoveId);
    }
}