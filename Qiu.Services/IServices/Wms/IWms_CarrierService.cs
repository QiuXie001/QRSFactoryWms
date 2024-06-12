using DB.Models;
using Qiu.Utils.Table;

namespace IServices.Wms
{
    public interface IWms_CarrierService : IBaseService<WmsCarrier>
    {
        Task<string> PageList(Bootstrap.BootstrapParams bootstrap);
    }
}