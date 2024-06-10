using DB.Models;
using Qiu.Core.Dto;

namespace IServices
{
    public interface IWms_InventoryServices : IBaseService<WmsInventory>
    {
        string PageList(PubParams.InventoryBootstrapParams bootstrap);

        string SearchInventory(PubParams.InventoryBootstrapParams bootstrap);
    }
}