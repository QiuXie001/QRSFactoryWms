using DB.Models;
using Qiu.Utils.Table;

namespace IServices.Wms
{
    public interface IWms_WarehouseService : IBaseService<WmsWarehouse>
    {
        Task<string> PageList(Bootstrap.BootstrapParams bootstrap);
    }
}