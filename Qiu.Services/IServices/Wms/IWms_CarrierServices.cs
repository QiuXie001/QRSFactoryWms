using DB.Models;
using Qiu.Utils.Table;

namespace IServices
{
    public interface IWms_CarrierServices : IBaseService<WmsCarrier>
    {
        string PageList(Bootstrap.BootstrapParams bootstrap);
    }
}