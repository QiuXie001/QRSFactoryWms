using DB.Models;
using Qiu.Utils.Table;

namespace IServices.Wms
{
    public interface IWms_DeliveryService : IBaseService<WmsDelivery>
    {
        Task<string> PageList(Bootstrap.BootstrapParams bootstrap);

        Task<bool> Delivery(WmsDelivery delivery);
    }
}