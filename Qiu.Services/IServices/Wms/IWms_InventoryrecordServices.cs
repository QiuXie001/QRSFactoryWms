using DB.Models;
using Qiu.Core.Dto;
using Qiu.Utils.Pub;

namespace IServices
{
    public interface IWms_InventoryrecordServices : IBaseService<WmsInventoryrecord>
    {
        string PageList(PubParams.InventoryBootstrapParams bootstrap);
    }
}