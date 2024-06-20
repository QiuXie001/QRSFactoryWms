using DB.Models;
using Qiu.Core.Dto;

namespace IServices.Wms
{
    public interface IWms_InventoryrecordService : IBaseService<WmsInventoryrecord>
    {
        Task<string> PageListAsync(PubParams.InventoryBootstrapParams bootstrap);
    }
}