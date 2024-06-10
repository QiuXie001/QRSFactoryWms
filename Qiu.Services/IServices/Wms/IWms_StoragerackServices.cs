using DB.Models;
using Qiu.Utils.Table;

namespace IServices
{
    public interface IWms_StoragerackServices : IBaseService<WmsStoragerack>
    {
        string PageList(Bootstrap.BootstrapParams bootstrap);
    }
}