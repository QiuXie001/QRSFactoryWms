using DB.Models;
using Qiu.Utils.Table;

namespace IServices
{
    public interface IWms_DeliveryServices : IBaseService<WmsDelivery>
    {
        string PageList(Bootstrap.BootstrapParams bootstrap);

        bool Delivery(WmsDelivery delivery);
    }
}