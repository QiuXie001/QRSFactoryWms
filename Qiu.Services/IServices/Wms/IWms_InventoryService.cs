using DB.Models;
using Qiu.Core.Dto;

namespace IServices.Wms
{
    public interface IWms_InventoryService : IBaseService<WmsInventory>
    {
        Task<string> PageListAsync(PubParams.InventoryBootstrapParams bootstrap);

        Task<string> SearchInventory(PubParams.InventoryBootstrapParams bootstrap);
    }
}