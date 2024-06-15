using DB.Models;
using Qiu.Utils.Table;

namespace IServices.Wms
{
    public interface IWms_DeliveryService : IBaseService<WmsDelivery>
    {
        Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap);

        Task<(bool,string)> DeliveryAsync(WmsDelivery delivery);
    }
}