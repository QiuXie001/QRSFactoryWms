using DB.Models;
using Qiu.Utils.Table;

namespace IServices
{
    public interface IWms_WarehouseServices : IBaseService<WmsWarehouse>
    {
        string PageList(Bootstrap.BootstrapParams bootstrap);
    }
}